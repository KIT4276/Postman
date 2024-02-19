<h1> Postman</h1>

<h2>A story about a brave postman who delivers packages despite the zombie invasion and the real danger of contracting a deadly virus.
 <h3>Task:</h3>
<p>Deliver packages without being killed by zombies and without dying from infection</p>

<br><a href="https://drive.google.com/file/d/1SCBgRyZrXjXFVe1PbejAAbFg-m5piQmA/view?usp=sharing">Link to a build</a></h2>

 <h3> Platform:</h3>
 Android

 <h3> Implemented mechanics:</h3>
 <p>
 1. Moving a character and turning it in the direction of movement<br>
 2. Camera follows character with adjustable lag<br>
 3. At the beginning of the game, a given number of enemies appear on the stage in given positions(from Zenject pool)<br>
 4. At the beginning of the game, a specified number of first aid kits (to restore health) appear on the stage in a given position (from Zenject pool)<br>
 5. The health of the player and enemies, which decreases when a weapon hits an adjustable area (HitBox)<br>
 6. The minimum number of enemies and first aid kits existing in the game is predetermined. Once their number reaches this value, new ones appear at all spawn points that are not occupied<br>
 7. When a player gets into an enemy's aggro zone, they will begin to chase him and stop when he leaves this zone<br>
 8. Enemies attack when the player enters the attack zone<br>
 9. When a player enters a trigger near a post office building, they will receive an address where the package should be delivered<br>
 10. When a player enters the trigger of the desired address, he receives money<br>
 11. After a successful enemy attack, infection begins, which ultimately leads to death<br>
 12. Infection can be cured for money<br>
 13. There is a translucent map on the screen, with player and destination markers<br>
 14. There are save points near the post office (health, delivered packages, money)<br>
 15. At the beginning of the game, the start menu is loaded, where you can start a new game or load the last saved one<br>
 </p>
 
 <h3>Used in the project:</h3>
 <p>
  Zenject<br>
  AI Navigation<br>
  Unity UI<br>
  SimpleInput<br>
 </p>

<h3> Control for Windows:</h3>
 WASD - moving<br>
 cursor - rotation<br>
 LMB - attack<br>

 <h3> Control for Android:</h3>
 arrows on head-up display - moving, rotation<br>
 button on head-up display - attack<br>


 
