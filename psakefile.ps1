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

task Nuget -depends SetReleaseBuild, Clean, Compile

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


function global:copy_files($source,$destination,$exclude=@()){    
    create_directory $destination
    Get-ChildItem $source -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
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

