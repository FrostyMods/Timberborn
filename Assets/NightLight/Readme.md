<p>I've noticed a number of streamers complaining about night time in Timberborn.</p>
<p>This is a simple mod to to remove night time lighting from the game. When night rolls around, the beavers will still sleep as usual, but the sunset lighting will persist through the night. No more darkness!</p>
<p>That's not all though. Every period of the day can be customized (read <strong>Advanced Usage</strong> to learn how).</p>
<h2>Basic Usage</h2>
<ol>
<li>Install the mod</li>
<li>Rejoice in the lack of night</li>
</ol>
<h2>Advanced Usage</h2>
<p>This is just a simple mod that aims to banish the night. As such, it doesn't have an in-game UI (leave a comment if you'd like one), but it <em>does</em> have a config file!</p>
<p><strong>Finding and opening the config file</strong></p>
<ol>
<li>Install the mod</li>
<li>Navigate to the mod's install location...
<ul>
<li>Open Timberborn in your Steam Library</li>
<li>Click on the cog icon, then "Manage", then "Browse local files"</li>
<li>Once your file explorer has opened up to the Timberborn directory, go to:
<ul>
<li><code>BepinEx &gt; plugins &gt; nightlight_2833428... &gt; NightLight &gt; configs</code></li>
</ul>
</li>
</ul>
</li>
<li>Open <code>LightingConfig.json</code> in Notepad, VS Code, etc. and read on!</li>
</ol>
<p><strong>Editing the config file</strong></p>
<p>To change the lighting used during a specific time of day, change it's value to 0 (sunrise), 1 (day), 2 (sunset), or 3 (night).</p>
<h3>Examples</h3>
<pre><code>// This will replace the night time lighting with sunset
// This is how the mod is configured by default
{"Sunrise":0,"Day":1,"Sunset":2,"Night":2}

// This will change the lighting so it's always day time
{"Sunrise":1,"Day":1,"Sunset":1,"Night":1}</code></pre>
<h2>Installing</h2>
<p>Follow <a title="Timberborn Mod Installation Guide" href="https://mod.io/g/timberborn/r/how-to-install-mods" target="_blank" rel="noopener noreferrer">this guide</a> for installing mods in Timberborn. You'll also need <a href="https://mod.io/g/timberborn/m/timberapi" target="_blank" rel="noopener noreferrer">TimberAPI</a> installed.</p>
<h2>Feedback</h2>
<p>Feel free to reach out on Discord if you're having issues with this mod or want suggest changes, etc. You can find me in the <a href="https://discord.com/channels/558398674389172225/888491376143134760" target="_blank" rel="noopener noreferrer">#modding-and-whatnot </a>channel, or you can DM me (frost#3443).</p>