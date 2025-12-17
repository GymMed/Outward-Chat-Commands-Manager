<h1 align="center">
    Outward Mod Pack Template
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Outward-Mod-Pack-Template/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	A template for creating a C# Outward Mod.
</div>

## How to use

1. Either clone/download the repository with Git or GitHub Desktop, or simply download the code manually.
2. Open `src/OutwardModPackTemplate.sln` with any C# IDE (Visual Studio, Rider, etc)
3. Open the `Plugin.cs` file and follow the instructions
4. When you're ready, build the solution. It will be built to the `Release` folder (next to the `src` folder).
5. Take the DLL from the `Release` folder and put it in the `BepInEx/plugins/` folder. If you use r2modman, this can be found by going into r2modman settings and clicking on `Browse Profile Folder`.

## Tip

Place automatically `.dll` document to your `r2modman` profile for quicker testing.
Inside `src/OutwardModPackTemplate.csproj` and in `Project` tag you can add:

<pre><code>&lt;Target Name="PostBuild" AfterTargets="PostBuildEvent"&gt;
  &lt;Exec Command="call &amp;quot;$(ProjectDir)..\Public\DontZip\placeBuild.bat&amp;quot;" /&gt;
&lt;/Target&gt;</code></pre>

Make sure that you changed variables inside `..\Public\DontZip\placeBuild.bat`.

For further help, see the [Outward Modding Wiki](https://outward.fandom.com/wiki/Getting_Started_Developing_Mods), or join the [Outward Modding Discord](https://discord.gg/zKyfGmy7TR).
