#r "paket:
version 7.0.2
framework: net6.0
source https://api.nuget.org/v3/index.json

nuget Microsoft.Build 17.3.2
nuget Microsoft.Build.Framework 17.3.2
nuget Microsoft.Build.Tasks.Core 17.3.2
nuget Microsoft.Build.Utilities.Core 17.3.2
nuget Be.Vlaanderen.Basisregisters.Build.Pipeline 6.0.6 //"

#load "packages/Be.Vlaanderen.Basisregisters.Build.Pipeline/Content/build-generic.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO.FileSystemOperators
open ``Build-generic``

let assemblyVersionNumber = (sprintf "%s.0")
let nugetVersionNumber = (sprintf "%s")

let buildSolution = buildSolution assemblyVersionNumber
let publishSource = publish assemblyVersionNumber
let pack = packSolution nugetVersionNumber

supportedRuntimeIdentifiers <- [ "linux-x64" ] 

// Library ------------------------------------------------------------------------
Target.create "Lib_Build" (fun _ -> buildSolution "Be.Vlaanderen.Basisregisters.MessageHandling")

Target.create "Lib_Test" (fun _ -> testSolution "Be.Vlaanderen.Basisregisters.MessageHandling")

Target.create "Lib_Publish" (fun _ ->
    publishSource "Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple"
    publishSource "Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple"
    publishSource "Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer"
    publishSource "Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.SqlServer"
    publishSource "Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Producer"
)

Target.create "Lib_Pack" (fun _ -> pack "Be.Vlaanderen.Basisregisters.MessageHandling")

// --------------------------------------------------------------------------------
Target.create "PublishAll" ignore
Target.create "PackageAll" ignore

// Publish ends up with artifacts in the build folder
"DotNetCli"
==> "Clean"
==> "Restore"
==> "Lib_Build"
==> "Lib_Test"
==> "Lib_Publish"
==> "PublishAll"

// Package ends up with local NuGet packages
"PublishAll"
==> "Lib_Pack"
==> "PackageAll"

// Publish ends up with artifacts in the build folder
Target.runOrDefault "Lib_Test"
