var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts      = Directory("./artifacts/packages");
var packageVersion      = "3.1.0";

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] 
    {
        buildArtifacts
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
    var files = GetFiles("./src/**/*.*");
    CopyFiles(files, "./feed/content", true);
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