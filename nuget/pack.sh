#!/bin/sh
mono --runtime=v4.0 nuget.exe pack AiEffects_mac.nuspec -symbols -Prop Configuration=Release -verbosity detailed -basepath ./ -OutputDirectory ~/projects/nuget
