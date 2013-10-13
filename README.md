ao-ladderer
===========

Implant laddering tool for the sci-fi MMORPG Anarchy Online.

Background
----------

Characters in Anarchy Online (AO) have six abilities and many different skills.  
The abilities are:  

| Strength      |  Agility          |
| :--------     | :------------     |
| **Stamina**   | **Intelligence**  |
| **Sense**     | **Psychic**       |

Implants are items that require one of the six abilities and a skill called Treatment to equip.   
Implants have a Quality Level (QL) which determines the magnitude of the ability and Treatment required.  
There are 13 slots on a character where implants of the corresponding type can be equipped:    

| Eye             | Head      | Ear            |
| --------------- | --------- | -------------- |
| **Right-Arm**   | **Chest** | **Left-Arm**   |
| **Right-Wrist** | **Waist** | **Left-Wrist** |
| **Right-Hand**  | **Leg**   | **Left-Hand**  |
|                 | **Feet**  |                |

Implants are useful because they can contain Nano Clusters.  
Clusters provide beneficial modifications; the higher the implant QL, the greater the benefits.  
Implants have three slots for clusters, which can contain at most one cluster of the corresponding type:  

| Shining | Bright | Faded |
| ------- | ------ | ----- |

Each type of implant has a set of Shining, a set of Bright, and a set of Faded clusters available to it.  
Across all 13 Shining sets, a cluster for each skill and each ability occurs exactly once.    
This is true for the 13 Bright and 13 Faded sets as well.  
None of the 39 cluster sets is the same as another.  
Example implants:  

| Type | QL  | Ability Req., Val. | Treatment Req. | Shining, Val.  | Bright, Val.    | Faded, Val.  |
| ---- | --- | ------------------ | -------------- | -------------- | --------------- | ------------ |
| Feet | 1   | Agility, 6         | 11             | Concealment, 6 | Martial Arts, 3 | Duck-Exp, 2  |
| Head | 200 | Intelligence, 404  | 951            | Chemistry, 105 | Nano Pool, 63   | Tutoring, 42 |

Another equivalent representation of the above implants:  

| QL  | Shining     | Bright       | Faded    |
| --- | ----------- | ------------ | -------- |
| 1   | Concealment | Martial Arts | Duck-Exp |
| 200 | Chemistry   | Nano Pool    | Tutoring |

**Laddering** is the process of using implants to equip higher QL implants.  
Neither of the implants above is useful for laddering, since neither adds to any abilities or Treatment.  
The following implants might be useful in a laddering process:  

| Type  | QL  | Ability Req. | Shining   | Bright    | Faded    |
| ----- | --- | ------------ | --------- | --------- | -------- |
| Chest | ?   | Psychic      | Sense     | Psychic   | Strength |
| Head  | ?   | Intelligence | Treatment | Nano Pool | Sense    |

Perhaps the Head implant would be equipped to allow a character with surplus Psychic to equip a higher QL of the Chest.
Both together might allow for equipping a higher QL of some Sense-requiring implant desired by the character.  

  
**Twinking** is the process of equipping a character with hard-to-equip and/or hard-to-get items.  
For many characters, laddering implants is a part of this process.  

Usage
-----

For Windows 7 or 8 users, download the latest release, unzip, and double-click on Ladderer.exe.  

Choose your implant configuration and input the abilities and Treatment you'll have at the time of equipping.
I do this by getting an actual character and equipping it with gear I think will be good for implant laddering.
Make sure there are no important implant slots full; it's assumed all slots are empty, but R/L-Wrist and L-Hand don't matter.
There are options to cast buffs on yourself if you don't want to get them beforehand. Be careful not to double-count.
Then just press build and follow the steps from there. Be sure not to remove ladder implants until absolutely necessary.

Usefulness
----------

First, a comparison:

Implant configuration of a level 48 NT twinked by hand: 

This tool provides a decent approximation for people wanting to quickly equip a low to mid-level character.
I wouldn't use the tool for full-blown twinks, because it has several limitations:  

* 






