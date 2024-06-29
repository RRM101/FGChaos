if (Test-Path -Path "..\Build\Debug")
{
	rm -Path "..\Build\Debug" -Recurse
}

New-Item -Path "..\Build" -Name "Debug" -ItemType "directory"

Copy-Item "bin\Debug\net6.0\FGChaos.dll" -Destination "..\Build\Debug"
Copy-Item "bin\Debug\net6.0\FGChaos.pdb" -Destination "..\Build\Debug"

Copy-Item -Path "..\Assets" -Destination "..\Build\Debug" -Recurse
Copy-Item -Path "..\Libs" -Destination "..\Build\Debug" -Recurse
exit 0