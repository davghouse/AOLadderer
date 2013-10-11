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

An equivalent representation of the above implants, with no redundancy:  

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

In AO, **twinking** is the process of equipping a character with hard-to-equip and/or hard-to-get items.  
For many characters, laddering implants is a part of this process.  
