var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts      = Directory("./artifacts/packages");
var feedDirectory       = Directory("./feed/content");
var packageVersion      = "3.3.0";

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] 
    {
        buildArtifacts,
        feedDirectory

    });
});

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings 
    {
        Configuration = configuration,
    };

    var projects = GetFiles("./src/**/*.csproj");
    foreach(var project in projects)
    {
        DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    }
});

///////////////////////////////////////////////////////////////////////////////
// Copy
///////////////////////////////////////////////////////////////////////////////
Task("Copy")
    .IsDependentOn("Clean")
    .Does(() =>
{
    CreateDirectory("./feed/content");

    // copy the single csproj templates
    var copyFiles = GetFiles("./src/**/*.*");
    CopyFiles(copyFiles, feedDirectory, true);

    // clean projects bin and obj folders, TODO: remove hard coded project name
    var cleanDirectories = new DirectoryPath[] 
    {
        "./feed/content/ProspaAspNetCoreApi/bin",
        "./feed/content/ProspaAspNetCoreApi/obj",
        "./feed/content/ProspaAspNetCoreApiNsb/bin",
        "./feed/content/ProspaAspNetCoreApiNsb/obj",
    };

    foreach(var directory in cleanDirectories)
    {
        if (DirectoryExists(directory))
        {
            DeleteDirectory(directory, new DeleteDirectorySettings { Recursive = true, Force = true });
        }
    }
});

///////////////////////////////////////////////////////////////////////////////
// Pack
///////////////////////////////////////////////////////////////////////////////
Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Copy")
    .Does(() =>
{
    var settings = new NuGetPackSettings
    {
        Version = packageVersion,
        OutputDirectory = buildArtifacts
    };

    NuGetPack("./feed/Prospa.Templates.nuspec", settings);
});


Task("Default")
  .IsDependentOn("Pack");

RunTarget(target);