$script:project_config = "Debug"
properties {
    $configuration = 'Debug'
	$project_name = "DemoApp"
	if(-not $version)
    {
        $version = "0.0.0.1"
    }
	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$temp_package_dir = "$build_dir\temp"
	$package_dir = "$build_dir\latestVersion"
	$source_dir = "$base_dir\src"
	$test_dir = "$build_dir\test"
	$result_dir = "$build_dir\results"
}

task default -depends InitialPrivateBuild, WarnSlowBuild

task help {
	Write-Help-Header
	Write-Help-Section-Header "Comprehensive Building"
	Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
	Write-Help-Footer
	exit 0
}

#These are the actual build tasks. They should be Pascal case by convention
task InitialPrivateBuild -depends Clean, Compile


task Nuget -depends SetReleaseBuild, Clean, Compile, PackageYayCore

task SetReleaseBuild {
    $script:project_config = "Release"
}

task CommonAssemblyInfo {
    create-commonAssemblyInfo "$version" $project_name "$source_dir\CommonAssemblyInfo.cs"
}


task Compile -depends Clean { 
    exec { dotnet build .\\src\\Yay.Enumerations --configuration $configuration /nologo }
}

task Clean {
	delete_directory $temp_package_dir
    delete_directory $build_dir
    create_directory $test_dir 
    create_directory $result_dir
    create_directory $package_dir
}

task PackageYayCore -depends SetReleaseBuild, Compile {
    delete_directory $temp_package_dir

    #dlls
    copy_files "$source_dir\Yay.Enumerations\bin\Release\" "$temp_package_dir\lib"

	#nuget spec (and any other needed nuget thing)
	copy_files "$base_dir\nuget\Yay.Enumerations\" "$temp_package_dir"
    
	$nuspec_file = "$temp_package_dir\Yay.Enumerations.nuspec"
	package-for-nuget $nuspec_file $package_dir
}

task WarnSlowBuild {
	Write-Host ""
	Write-Host "Warning: " -foregroundcolor Yellow -nonewline;
	Write-Host "The default build you just ran is primarily intended for initial "
	Write-Host "environment setup. While developing you most likely want the quicker dev"
	Write-Host "build task. For a full list of common build tasks, run: "
	Write-Host " > build.bat help"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions added by Headspring for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
	Write-Host ""
	Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
	Write-Host " HELP " -foregroundcolor Green  -nonewline; 
	Write-Host "********************************"  -foregroundcolor DarkGreen
	Write-Host ""
	Write-Host "This build script has the following common build " -nonewline;
	Write-Host "task " -foregroundcolor Green -nonewline;
	Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
	Write-Host ""
	Write-Host " For a complete list of build tasks, view default.ps1."
	Write-Host ""
	Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
	Write-Host ""
	Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
	Write-Host "  > " -nonewline;
	Write-Host "$alias" -foregroundcolor Green -nonewline; 
	Write-Host " = " -nonewline; 
	Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------
function package-for-nuget($nuspec_file,$package_dir) {
    exec { & .\tools\NuGet.exe pack $nuspec_file -OutputDirectory $package_dir }
}

function run_tests([string]$pattern) {
    
    $items = Get-ChildItem -Path $test_dir $pattern
    $items | %{ run_nunit $_.Name }
}

function global:zip_directory($directory,$file) {
    write-host "Zipping folder: " $test_assembly
    delete_file $file
    cd $directory
    & "$base_dir\lib\7zip\7za.exe" a -mx=9 -r $file
    cd $base_dir
}

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name) {
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create_directory($directory_name) {
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:run_nunit ($test_assembly) {
	$assembly_to_test = $test_dir + "\" + $test_assembly
	$results_output = $result_dir + "\" + $test_assembly + ".xml"
    write-host "Running NUnit Tests in: " $test_assembly
    exec { & lib\nunit\nunit-console-x86.exe $assembly_to_test /nologo /nodots /xml=$results_output /exclude=DataLoader}
}

function global:load_test_data ($test_assembly) {
	$assembly_to_test = $test_dir + "\" + $test_assembly
    write-host "Running DataLoader NUnit Tests in: " $test_assembly
    exec { & lib\nunit\nunit-console-x86.exe $assembly_to_test /nologo /nodots /include=DataLoader}
}

function global:Copy_and_flatten ($source,$include,$dest) {
	ls $source -include $include -r | cp -dest $dest
}

function global:copy_all_assemblies_for_test($destination){
	$bin_dir_match_pattern = "$source_dir\*\bin\$project_config"
	create_directory $destination
	Copy_and_flatten $bin_dir_match_pattern *.exe $destination
	Copy_and_flatten $bin_dir_match_pattern *.dll $destination
	Copy_and_flatten $bin_dir_match_pattern *.config $destination
	Copy_and_flatten $bin_dir_match_pattern *.pdb $destination
	Copy_and_flatten $bin_dir_match_pattern *.sql $destination
	Copy_and_flatten $bin_dir_match_pattern *.xlsx $destination
}

function global:copy_website_files($source,$destination){
    $exclude = @('*.user','*.dtd','*.tt','*.cs','*.csproj') 
    copy_files $source $destination $exclude
	delete_directory "$destination\obj"
}

function global:copy_files($source,$destination,$exclude=@()){    
    create_directory $destination
    Get-ChildItem $source -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:Convert-WithXslt($originalXmlFilePath, $xslFilePath, $outputFilePath) {
   ## Simplistic error handling
   $xslFilePath = resolve-path $xslFilePath
   if( -not (test-path $xslFilePath) ) { throw "Can't find the XSL file" } 
   $originalXmlFilePath = resolve-path $originalXmlFilePath
   if( -not (test-path $originalXmlFilePath) ) { throw "Can't find the XML file" } 
   #$outputFilePath = resolve-path $outputFilePath -ErrorAction SilentlyContinue 
   if( -not (test-path (split-path $originalXmlFilePath)) ) { throw "Can't find the output folder" } 

   ## Get an XSL Transform object (try for the new .Net 3.5 version first)
   $EAP = $ErrorActionPreference
   $ErrorActionPreference = "SilentlyContinue"
   $script:xslt = new-object system.xml.xsl.xslcompiledtransform
   trap [System.Management.Automation.PSArgumentException] 
   {  # no 3.5, use the slower 2.0 one
      $ErrorActionPreference = $EAP
      $script:xslt = new-object system.xml.xsl.xsltransform
   }
   $ErrorActionPreference = $EAP
   
   ## load xslt file
   $xslt.load( $xslFilePath )
     
   ## transform 
   $xslt.Transform( $originalXmlFilePath, $outputFilePath )
}

function global:create-commonAssemblyInfo($version,$applicationName,$filename) {
    "using System.Reflection;
using System.Runtime.InteropServices;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyCopyrightAttribute(""Copyright 2012"")]
[assembly: AssemblyProductAttribute(""$applicationName"")]
[assembly: AssemblyCompanyAttribute("""")]
[assembly: AssemblyConfigurationAttribute(""release"")]
[assembly: AssemblyInformationalVersionAttribute(""$version"")]"  | out-file $filename -encoding "ASCII"    
}


function script:poke-xml($filePath, $xpath, $value, $namespaces = @{}) {
    [xml] $fileXml = Get-Content $filePath
    
    if($namespaces -ne $null -and $namespaces.Count -gt 0) {
        $ns = New-Object Xml.XmlNamespaceManager $fileXml.NameTable
        $namespaces.GetEnumerator() | %{ $ns.AddNamespace($_.Key,$_.Value) }
        $node = $fileXml.SelectSingleNode($xpath,$ns)
    } else {
        $node = $fileXml.SelectSingleNode($xpath)
    }
    
    Assert ($node -ne $null) "could not find node @ $xpath"
        
    if($node.NodeType -eq "Element") {
        $node.InnerText = $value
    } else {
        $node.Value = $value
    }

    $fileXml.Save($filePath) 
}

function usingx {
    param (
        $inputObject = $(throw "The parameter -inputObject is required."),
        [ScriptBlock] $scriptBlock
    )

    if ($inputObject -is [string]) {
        if (Test-Path $inputObject) {
            [void][system.reflection.assembly]::LoadFrom($inputObject)
        } elseif($null -ne (
              new-object System.Reflection.AssemblyName($inputObject)
              ).GetPublicKeyToken()) {
            [void][system.reflection.assembly]::Load($inputObject)
        } else {
            [void][system.reflection.assembly]::LoadWithPartialName($inputObject)
        }
    } elseif ($inputObject -is [System.IDisposable] -and $scriptBlock -ne $null) {
        Try {
            &$scriptBlock
        } Finally {
            if ($inputObject -ne $null) {
                $inputObject.Dispose()
            }
            Get-Variable -scope script |
                Where-Object {
                    [object]::ReferenceEquals($_.Value.PSBase, $inputObject.PSBase)
                } |
                Foreach-Object {
                    Remove-Variable $_.Name -scope script
                }
        }
    } else {
        $inputObject
    }
}