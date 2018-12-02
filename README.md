﻿AO Ladderer
===========

Implant laddering tool for the sci-fi MMORPG Anarchy Online. 

Latest release [here](https://github.com/davghouse/AOLadderer/releases/tag/v2.2.0) (built for W10, but it probably works on W7 too). If you can't use that, there's a slower web-based version [here](https://ao-ladderer.github.io) (requires a modern browser), and an old cross-platform version [here](https://github.com/davghouse/AOLadderer/releases/tag/v1.3.3).

![Build](/Screenshots/Build.png)
![Basic Ladder](/Screenshots/BasicLadder.png)
![Basic Shopping](/Screenshots/BasicShopping.png)

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
Clusters provide beneficial modifications; the higher the implant QL, the greater the benefits.
Implants have three slots for clusters, which can contain at most one cluster of the corresponding type:

| Shiny | Bright | Faded |
| ----- | ------ | ----- |

Each type of implant has a set of Shiny, a set of Bright, and a set of Faded clusters available to it.
Across all 13 Shiny sets, a cluster for each skill and each ability occurs exactly once.
This is true for the 13 Bright and 13 Faded sets as well.
Example implants:

| Slot | QL  | Ability Req      | Treatment Req  | Shiny         | Bright         | Faded       |
| ---- | --- | ---------------- | -------------- | ------------- | -------------- | ----------- |
| Feet | 1   | 6 Agility        | 11             | 6 Concealment | 3 Martial Arts | <Empty>     |
| Head | 200 | 404 Intelligence | 951            | 105 Chemistry | 63 Nano Pool   | 42 Tutoring |

Equivalent, minimal representation of the above implants:

| QL  | Shiny       | Bright       | Faded    |
| --- | ----------- | ------------ | -------- |
| 1   | Concealment | Martial Arts | <Empty>  |
| 200 | Chemistry   | Nano Pool    | Tutoring |

**Implant laddering** is the process of using implants (and other items) to equip higher QL implants.
Neither of the implants above is directly useful for laddering, since neither adds to any abilities or Treatment.
The following ladder implants might be useful in a laddering process:

| Slot  | QL  | Ability Req  | Shiny     | Bright    | Faded    |
| ----- | --- | ------------ | --------- | --------- | -------- |
| Chest | ?   | Psychic      | Sense     | Psychic   | Strength |
| Head  | ?   | Intelligence | Treatment | Nano Pool | Sense    |

Perhaps the Head implant would be equipped to allow a character with surplus Psychic (meaning there exists an implant for which they have enough Psychic but not enough Treatment) to equip a higher QL of the Psychic-requiring Chest.
Both together might allow for equipping a higher QL of some Sense-requiring (and of course, Treatment-requiring) implant.

**Twinking** is the process of equipping a character with hard-to-equip and/or hard-to-get items.
For many characters, laddering implants is a part of this process.

Usage
-----

For Windows 10 users, download the latest release, extract, and double-click on the AO Ladderer exe.
Earlier versions of Windows aren't supported, but there's a chance it might work on them.
Users without Windows 10 can use a slower web-based version [here](https://ao-ladderer.github.io) (requires a modern browser), or an old cross-platform version [here](https://github.com/davghouse/AOLadderer/releases/tag/v1.3.3).

Choose your implant configuration and input the abilities and Treatment you'll have at the time of equipping.
I do this by having an actual character and equipping it with gear I think will be good for implant laddering.
You don't need to tell it the actual abilities you see in your skill window: see the Tips section below for more details.
Make sure there are no important implant slots full; it's assumed all slots are empty, but R/L-Wrist and L-Hand don't matter.
There are options to cast buffs on yourself if you don't want to get them in-game beforehand. Be careful not to double-count.
Then run one of the ladderers, build the exact implants shown, and equip them in the displayed order.
Don't remove ladder implants until absolutely necessary, and remember to swap Explosifs if you're using them.

And as an optional feature you can ctrl+left-click on the names of implant slots to mark them as unavailable.
For example, you might want to take special measures to equip Touch of the Gripper, and then ctrl+left-click the Right-Arm slot to make sure it's not used for laddering.
You'd then want to add to your Strength the 29 points that Touch of the Gripper provides, because otherwise the ladderer won't know about it.
You can also use this functionality to create a highly customized ladder process like you're used to, laddering up single ladder implants at a time, then marking the slot as unavailable.
Instead of the ladderer giving you a complete ladder process, you can just use it to take care of the math you were doing before.

Usefulness
----------

Comparing implant configurations for a level 48 froob Nano-Technician:

| Strategy             | Average QL | Link                                       | Matter Crea |
| -------------------- | ---------- | ------------------------------------------ | ----------- |
| Direct               | 125.00     | http://auno.org/ao/equip.php?saveid=192649 | 138         |
| Twink-by-hand        | 140.46     | http://auno.org/ao/equip.php?saveid=192647 | 147         |
| AO Ladderer (v1.3.3) | 140.62     | http://auno.org/ao/equip.php?saveid=193165 | 145         |

The first strategy uses no ladder implants, just equipping required implants in the order which maximizes their average QL (this order is non-obvious).
The second strategy was done by hand, twinking naturally. The third uses the basic ladder.
Matter Crea is listed because it's the most important stat for that specific character.
The average QL from the ladderer is slightly higher, but the Matter Crea is 2 points less, which could be a big deal.

The ladderer provides a decent approximation for people wanting to equip a low to mid-level character.
It is very good at being frugal with the amount of ladder implants that you have to create.
For the basic ladder, you'll never have to build more than 10 ladder implants, and it'll usually be less than that.

Tips
----
It's easy to modify the inputs a bit to simulate swapping items.
For instance, on low level characters I almost always twink with Explosifs.
These can be equipped on most characters with zero implant support.
For this reason, and because implants only require a single ability, it's fine to pretend we have a pair of Explosifs that adds +20 to every ability, rather than just Agi/Sen, Str/Sta, or Int/Psy.
There is one caveat: the Str/Sta combination doesn't provide any Treatment trickle, so either don't add +20 to Str/Sta, or don't add +2.5 to your Treatment.
This swap is so common the ladderer supports it directly.
Just remember, if you tell it about this swap you can't also tell it about Treatment buffing weapons like the Quartz rifle.
There's no way to tell the ladderer (yet) that sometimes you have +10 Treatment and +8 Sta, and other times you have +20 Str/Sta/Agi/Sen/Int/Psy.
You could tell it about a +25 Tsakachumi and Explosifs though; +25 Agi, +20 Str/Sta/Sen/Int/Psy, as long as you can get the Tsak on without implants.

This same strategy can be applied to any equipment slots not being used for Treatment buffing.
Just be able to equip the swaps without implant support, and don't over-count Treatment trickle.
It can also be applied to buffs; you don't have to choose between Prodigious Strength and Iron Circle.
One gives +40 Str, one gives +20 Sta and Str.
For the purposes of laddering implants they effectively provide +40 Str and +20 Sta; cancel one when you need the other.
For convenience the ladderer supports this combined buff directly.

**You'll have to tell the ladderer about your ability to swap items (other than Explosifs), by manually increasing your starting abilities.
Don't think of these as your actual abilities, think of them as your effective abilities for the purposes of equipping implants.
They are your abilities that can be gotten simultaneously not with the other listed abilities, but with the listed Treatment.**
I almost never bother telling it about any swaps other than Explosifs.

If R/L-Wrist and L-Hand are important for your setup, consider using the ladderer on those three alone, and then doing the rest of your implants.
This is OK because those aren't ladder slots, so the ladderer will never use them when laddering up the rest.

If you really want, you can build each implant in the ladder one at a time.
Before building an implant, swap around whatever gear you want in order to maximize the QL you can equip, and build that QL instead.
Then you can just think of the ladder as a loose guide.
It will represent a good order in which to equip implants, where you know you'll get final implants of at least the listed QLs.
This could be easier for some people than trying to accurately set their effective initial abilities/Treatment.

And read about ctrl+left-clicking in the Usage section above if you haven't already.

Algorithm
---------

Intuitively, the basic ladder process uses a greedy algorithm that proceeds recursively as follows:

Equip the ladder implant that would yield the highest average final implant QL if we were to equip all of our final implants immediately afterwards (in the optimal order--found by brute force).
Stop equipping ladder implants when we run out of slots for them, or when the addition of the next one no longer increases the average QL.

The advanced ladder process adds another layer of recursion on top:

Get the set of ladder implants found by the basic ladder process, and then ladder those implants up via the basic ladder process (and then use those laddered ladder implants to equip the final implants).
Going any further than one level deep isn't worth it, as we very quickly run into diminishing returns.

More details:

There's a base solver to find the ordering over a set of slot-distinct implants that maximizes their average QL.
Such a set of implants has no more than 10 ladder implants, if every slot that can contain a ladder implant does.
There's no reason to equip non-ladder implants before all ladder implants are equipped.
To find the ordering it looks at every possible ordering of all the ladder implants in the set (≤ 10! = 3,628,800 orderings)
An additional requirement is that all implants in unoccupied slots must be equipped before implants in occupied slots.
The occupied slots are occupied by ladder implants we've found previously; it doesn't make sense to remove them first, because
they're there in order to improve the QL of implants equipped after them.
This cuts down the number of orderings in many cases, like from 10! to 5! * 5!.

The algorithm uses this base solver. Each step in the algorithm chooses the next ladder implant to equip.
This is done by considering each unoccupied laddering slot in turn. Each slot has a set of ladder implants
available to it. This set is broken down into subsets, where implants within the same subset differ
in their required ability, but modify the abilities and Treatment in the same way. From each subset the best implant
(the one requiring the ability we have the most of) is inserted, and tested. Testing runs the base solver on
the final implants, given the ladder implants we've found so far (which get unequipped and replaced by final implants in the base solver where necessary).
The implant yielding the highest average QL for the final implants is chosen and locked in, and the next step proceeds recursively.
The algorithm stops when no more laddering slots are available, or when the average QL from one step to the next doesn't increase.

Fortunately, there aren't many ladder implants. There are 33 subsets, with implants equal (from a laddering perspective)
up to their required ability. There are 60 total ladder implants considered by the algorithm.
Many ladder implants aren't included -- if their cluster specification is a subset of another's and they require the same ability, there would never be a reason to equip them.
