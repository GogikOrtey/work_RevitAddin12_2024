﻿sealed partial class Build
{
    const string Version = "0.0.2";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    protected override void OnBuildInitialized()
    {
        Configurations =
        [
            "Release*",
            "Installer*"
        ];

        InstallersMap = new()
        {
            {Solution.Installer, Solution.Application}
        };
    }
}