ao-ladderer
===========

Implant laddering tool for the sci-fi MMORPG Anarchy Online. 

Latest release [here](https://github.com/davghouse/ao-ladderer/releases/tag/v1.3.0).

![tab 1](/Screenshots/5.png)
![tab 2](/Screenshots/6.png)
![tab 3](/Screenshots/7.png)
![tab 4](/Screenshots/8.png)

Background
----------

Characters in Anarchy Online (AO) have six abilities and many different skills.  
The abilities are:  

| Strength      |  Agility          |
| :--------     | :------------     |
| **Stamina**   | **Intelligence**  |
| **Sense**     | **Psychic**       |

Implants are items that require one of the six abilities and a skill called Treatment to equip.   
Implants have a Quality Level (QL), which determines the amount of ability and Treatment required.  
There are 13 slots on a character where implants of the corresponding type can be equipped:    

| Eye             | Head      | Ear            |
| --------------- | --------- | -------------- |
| **Right-Arm**   | **Chest** | **Left-Arm**   |
| **Right-Wrist** | **Waist** | **Left-Wrist** |
| **Right-Hand**  | **Leg**   | **Left-Hand**  |
|                 | **Feet**  |                |

Implants are useful because they can contain Nano Clusters.  
Talking about an implant usually means talking about a cluster specification, with no QL specified.  
Clusters provide beneficial modifications; the higher the implant QL, the greater the benefits.  
Implants have three slots for clusters, which can contain at most one cluster of the corresponding type:  

| Shining | Bright | Faded |
| ------- | ------ | ----- |

Each type of implant has a set of Shining, a set of Bright, and a set of Faded clusters available to it.  
Across all 13 Shining sets, a cluster for each skill and each ability occurs exactly once.    
This is true for the 13 Bright and 13 Faded sets as well.  
Example implants:  

| Type | QL  | Ability Req., Val. | Treatment Req. | Shining, Val.  | Bright, Val.    | Faded, Val.  |
| ---- | --- | ------------------ | -------------- | -------------- | --------------- | ------------ |
| Feet | 1   | Agility, 6         | 11             | Concealment, 6 | Martial Arts, 3 | Duck-Exp, 2  |
| Head | 200 | Intelligence, 404  | 951            | Chemistry, 105 | Nano Pool, 63   | Tutoring, 42 |

Equivalent representation of the above implants:  

| QL  | Shining     | Bright       | Faded    |
| --- | ----------- | ------------ | -------- |
| 1   | Concealment | Martial Arts | Duck-Exp |
| 200 | Chemistry   | Nano Pool    | Tutoring |

**Implant laddering** is the process of using implants (and other items) to equip higher QL implants.  
Neither of the implants above is directly useful for laddering, since neither adds to any abilities or Treatment.  
The following ladder implants might be useful in a laddering process:  

| Type  | QL  | Ability Req. | Shining   | Bright    | Faded    |
| ----- | --- | ------------ | --------- | --------- | -------- |
| Chest | ?   | Psychic      | Sense     | Psychic   | Strength |
| Head  | ?   | Intelligence | Treatment | Nano Pool | Sense    |

Perhaps the Head implant would be equipped to allow a character with surplus Psychic (meaning there exists an implant for which they have enough Psychic but not enough Treatment) to equip a higher QL of the Chest.
Both together might allow for equipping a higher QL of some Sense-requiring (and of course, Treatment-requiring) implant.  

  
**Twinking** is the process of equipping a character with hard-to-equip and/or hard-to-get items.  
For many characters, laddering implants is a part of this process.  

Usage
-----

For Windows 7 or 8 users, download the latest release, unzip, and double-click on Ladderer.exe. It looks good in Windows Classic theme and bad elsewhere.

Choose your implant configuration and input the abilities and Treatment you'll have at the time of equipping. 
I do this by having an actual character and equipping it with gear I think will be good for implant laddering.
You don't need to tell it the actual abilities you see in your skill window: see the Tips section below for more details.
Make sure there are no important implant slots full; it's assumed all slots are empty, but R/L-Wrist and L-Hand don't matter.
There are options to cast buffs on yourself if you don't want to get them in-game beforehand. Be careful not to double-count.
Then press build and follow the steps from there. Don't remove ladder implants until absolutely necessary.

Usefulness
----------

Comparing implant configurations for a level 48 froob Nano-Technician:  

| Strategy     | Average QL | Link                                       | Matter Crea |
| ------------ | ---------- | ------------------------------------------ | ----------- |
| Twink        | 140.46     | http://auno.org/ao/equip.php?saveid=192647 | 147         |
| ao-ladderer  | 134.54     | http://auno.org/ao/equip.php?saveid=192648 | 145         |
| Direct       | 125.00     | http://auno.org/ao/equip.php?saveid=192649 | 138         |
| ao-ladderer + swap | 140.62 | http://auno.org/ao/equip.php?saveid=193165 | 145 | 

The first strategy was done by hand, twinking naturally. The second uses the tool.
The third uses no ladder implants, just equipping required implants in the order which maximizes the average QL.
For more information on the last strategy, see the Tips section below. 
   
This tool provides a decent approximation for people wanting to equip a low to mid-level character.

**Good things:**  
* Just one of each type of ladder implant can be used
* As a result, no more than 10 ladder implants will be used
* No more than 23 implant insertions
* No more than 10 implant removals
* Two simply-described, simply-presented steps; equipping, and reverse-removal
  
**Bad things:** 
* Knows nothing about non-implant equipment (main reason for lower average QL)
* Knows nothing about perks
* Maximizes over average QL, but some implants matter more and some less
* Requires implants of exact QLs, which is an annoying process and an infeasible one for some players

Tips
----
Swapping items isn't supported by the tool, but it's easy to modify the inputs a bit to partially simulate that. For instance,
on low level characters I almost always twink with Explosifs. These can be equipped on most characters with zero implant support.
For this reason, and because implants only require a single ability, it's fine to pretend we have a pair of Explosifs
that adds +20 to every ability, rather than just Agi/Sen, Str/Sta, or Int/Psy. There is one caveat; the Str/Sta 
combination doesn't provide any treatment trickle, so either don't add +20 to Str/Sta, or subtract 3 from your treatment. [**The basic Explosif's swap is supported as of 1.3.0**]

This same strategy can be applied to any equipment slots not being used for treatment buffing.
Just be able to equip the swaps without implant support, and don't overcount treatment trickle. It can also be applied to buffs; you don't have to choose between Prodigious Strength and Iron Circle . One gives +40 Strength, one gives +20 Stamina and Strength. Effectively, they provide +40 Strength and +20 Stamina; cancel one when you need the other. [**Combined IC/PS buff is supported as of 1.3.0**]

Continuing the above example of the 48 Nano-Technician:

| Strategy     | Average QL | Link                                       | Matter Crea |
| ------------ | ---------- | ------------------------------------------ | ----------- |
| ao-ladderer + swap | 140.62 | http://auno.org/ao/equip.php?saveid=193165 | 145 |

That's a big difference. The average QL is greater than the natural twink's, though the Matter Crea is still lower.
For a swap as simple as Explosifs, it's worth it. Equipping the implants isn't quite as easy, 
but it will be obvious when changing the type of the Explosifs is necessary.

**You'll have to tell the tool about your ability to swap items, by manually increasing your starting abilities. Don't think of these as your actual abilities, think of them as your effective abilities for the purposes of equipping implants.**

Another tip; if L/R wrists and left-hand are important for your setup, consider using the tool on those three alone, and then doing the rest of your implants. This is OK because those aren't ladder slots, so the tool will never use them when laddering up the rest.

Algorithm
---------

Intuitively, the 'Basic' solver uses a greedy algorithm that proceeds recursively as follows: Equip the ladder implant that, if we were to equip all of our required implants in all the currently unoccupied slots, and then equip any more required implants by removing ladder implants in the reverse order of their addition, would yield the highest average QL for the final implant set. Stop equipping ladder implants when we run out of slots for them, or when the addition of the next one no longer increases the average QL. The 'Advanced' solver adds another layer of recursion on top: it gets the set of ladder implants found by the basic solver, and then ladders those implants via the basic solver (and then uses those laddered ladder implants to equip the required implants). It short-circuits after one layer, because going further doesn't seem to be worth it. 


More details:

There's a base solver to find the ordering over a set of slot-distinct implants that maximizes their average QL.
Such a set of implants has no more than 10 ladder implants, if every slot that can contain a ladder implant does.
To find the ordering it looks at every possible ordering of all the ladder implants in the set (10! = 3,628,800).  
  
The algorithm uses this solver. Each step in the algorithm chooses the next ladder implant to equip.
This is done by considering each unoccupied laddering slot in turn. Each slot has a set of ladder implants
available to it. This set is broken down into subsets, where implants within the same subset differ
in their required ability, but modify the abilities and Treatment in the same way. From each subset the best implant
(the one requiring the ability we have the most of) is inserted, and tested. Testing runs the base solver on
the remaining required implants, and then removes in reverse the ladder implants, inserting a required implant
after each removal. The implant yielding the highest average QL for the required implants is chosen and locked in, and the next step proceeds recursively.
The algorithm stops when no more laddering slots are available, or when the average QL from one step to the next 
doesn't increase.  

Fortunately, there aren't many ladder implants. There are 33 subsets, with implants equal (from a laddering perspective)
up to their required ability. There are 63 total ladder implants considered by the algorithm. 
Some ladder implants aren't included -- if their cluster specification is a subset of another's and they require the same ability.

Ideas for improvement:

As a simple modification of the above algorithm, I've considered looking two (or more) implants ahead rather than one. For example, greedily choose ordered pairs of ladder implants rather than a single ladder implant. This (roughly) multiplies the runtime by the number of ladder implants, which is significant.

To-do
------------
* Add more support for swapping weapons, armor, and items
* ~~Remove unused code~~
* ~~Allow exporting configurations to~~ [~~Auno~~] (http://auno.org)
* ~~Allow saving and loading configurations~~
* Add support for Jobe clusters
* ~~Consider adding support for refined implants~~
* Redesign UI so it uses a table widget and works under magnification
* Build on Linux
* Explore better algorithms
* Incorporate into a general twinker for Anarchy Online
