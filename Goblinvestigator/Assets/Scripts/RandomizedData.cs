using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizedData : MonoBehaviour {

	private Dictionary<string, GoblinData> _goblinDict = new Dictionary<string, GoblinData>();
	public Dictionary<string, GoblinData> GoblinDict
	{
		get
		{
			return _goblinDict;
		}
		set
		{
			//set by GameSetup script
			_goblinDict = value;
		}
	}

	private Dictionary<string, GameObject> dict_Weapons = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> Dict_Weapons
	{
		get {
			if(dict_Weapons.Count <= 0)
			{
				dict_Weapons.Add("axe", axe);
				dict_Weapons.Add("brick", brick);
				dict_Weapons.Add("club", club);
				dict_Weapons.Add("dagger", dagger);
				dict_Weapons.Add("spear", spear);
				dict_Weapons.Add("sword", sword);
			}

			return dict_Weapons; }
	}


	List<string> names = new List<string>();
	List<string> titles = new List<string>();
	List<string> weapons = new List<string>();

	public GameObject axe;
	public GameObject brick;
	public GameObject club;
	public GameObject dagger;
	public GameObject spear;
	public GameObject sword;

	public GameObject GetWeaponPrefab(string weaponName)
	{
		Dictionary<string, GameObject> shit = Dict_Weapons;
		return shit[weaponName];
	}

	//goblin A = the one you are speaking to
	//goblin B = the the goblin you are asking A about, specifically about how B felt about the victim
	public string GetDialogue3(string goblinB_name, string goblinB_weapon, string victim_name, string feel)
	{
		//Load Dictionaries and lists
		Dictionary<string, List<string>> DialogueDict = new Dictionary<string, List<string>>();
		List<string> hate = new List<string>();
		List<string> love = new List<string>();
		List<string> murdered = new List<string>();
		List<string> neutral = new List<string>();
		List<string> unset = new List<string>();	//to show when an option is unaccounted for in #3


		//HATE
		hate.Add(goblinB_name + " sure hated " + victim_name + ".");
		hate.Add(goblinB_name + " was always fighting with " + victim_name + ".");
		hate.Add(goblinB_name + " threw darts ata  photo of " + goblinB_name + " every morning before breakfast.");
		hate.Add("Once, " + victim_name + " dropped " + goblinB_name + "'s brand new PS4 and smashed it to bits and pieces, then didn't even apologize!");
		hate.Add(goblinB_name + " and " +victim_name + " were friends for years, but then had a major falling out after " + victim_name + " punched is little kitty cat.  They hated each other after that.");
		hate.Add("Yeah, " + goblinB_name + " hated " + victim_name + ", alright.  " + goblinB_name + " took a dump in " + victim_name + "'s bed many times.");
		hate.Add(goblinB_name + " always resented " + victim_name + " for wetting his bed during sleepovers.");
		hate.Add(victim_name + " pissed in + " + goblinB_name + "'s cereal, one time.");

		//LOVE
		love.Add(goblinB_name + " and " + victim_name + " were like two peas in a pod.");
		love.Add(goblinB_name + " sure looked up to " + victim_name + ".");
		love.Add("I used to see " + goblinB_name + " and " + victim_name + " holding hands and skipping in the rain together.");
		love.Add(goblinB_name + " and " + victim_name + " were always partners in Chemistry.");
		love.Add("Don't tell anyone, but " + goblinB_name + " loved " + victim_name + ".");
		love.Add("When I wandered past " + goblinB_name + "'s house a few nights ago, I heard them moaning " + victim_name + " over and over again.");

		//MURDERED
		murdered.Add("I saw " + goblinB_name + " come charging at " + victim_name + " with that " + goblinB_weapon + ", and I just took off, man!");
		murdered.Add("I saw " + goblinB_name + " washing blood off their " + goblinB_weapon + ".");
		murdered.Add(goblinB_name + " did it!  I saw it!  Took that " + goblinB_weapon + " right down on " + victim_name + "'s head!");
		murdered.Add("Don't say nothin', but " + goblinB_name + " did it.  You'll find that bloody " + goblinB_weapon + " around here somewhere.");

		//NEUTRAL
		neutral.Add("I never really saw " + goblinB_name + " and " + victim_name + " together.");
		neutral.Add(goblinB_name + " never really mentioned " + victim_name + " to me.");
		neutral.Add("I dunno, man.  But I think "+goblinB_name+ " and " + victim_name + " were pretty chill.");
		neutral.Add(goblinB_name + " and " + victim_name + "?  How would I know?");
		neutral.Add(goblinB_name + " and " + victim_name + " didn't have a bad relationship.");
		neutral.Add(goblinB_name + " and " + victim_name + " were just acquaintances, I think.");
		neutral.Add(goblinB_name + " and " + victim_name + " didn't have any hard feelings about each other.");

		//ERROR CHECK
		unset.Add("FIX THIS");



		DialogueDict["hate"] = hate;
		DialogueDict["love"] = love;
		DialogueDict["murdered"] = murdered;
		DialogueDict["neutral"] = neutral;
		DialogueDict["UNSET FIX THIS"] = unset;

		int randomDialogueIndex = Random.Range(0, DialogueDict[feel].Count);    //get random index in range of correct set of dialogue options
		List<string> currentList = DialogueDict[feel];                          //get current list
		return currentList[randomDialogueIndex];                                //return appropriate dialogue	

	}

	public string GetEmotionalDialogue(string feel, string currentName)
	{
		Dictionary<string, List<string>> DialogueDict = new Dictionary<string, List<string>>();
		List<string> hate = new List<string>();
		List<string> love = new List<string>();
		List<string> neutral = new List<string>();

		//load hate
		hate.Add(currentName + " is a jerk.");
		hate.Add(currentName + " ran over my dog.");
		hate.Add("I fucking hate " + currentName + ".");
		hate.Add("That " + currentName + " is nothing but trouble.");
		hate.Add(currentName + " really sucks.");
		hate.Add(currentName + "?  Super nice goblin.  But I hated them.");
		hate.Add("As far as goblins go, that " + currentName + " is the worst.");
		hate.Add(currentName + ", what a jerk.");
		hate.Add("I haven't spoken to " + currentName + " since they slept with my goblinfriend.");
		hate.Add("F#$% " + currentName + "!!!!");
		hate.Add(currentName + ", that little S#*!");
		hate.Add("What do I think of " + currentName + "?  A jerkweed, that's what I think.");
		hate.Add(currentName + " is useless.  Utterly useless as a goblin.  Utterly.  Useless.  Utterly, utterly useless.  Yep.");
		hate.Add(currentName + " ate my waffles when I wasn't looking once.  *sob*");
		hate.Add("Mom told me never to hang out with " + currentName + ".");
		hate.Add(currentName + "!?  How dare you speak that name to me!?");
		hate.Add("I don't ever want to see " + currentName + "'s hideous face again.");
		hate.Add(currentName + "...Please don't ask.  I don't want to hear that ugly name.");
		hate.Add(currentName + " is a piece of crap.  A giant piece of crap.");
		hate.Add("If you look up 'slimy' in the dictionary, you'll see that it reads, 'See also:  " + currentName + "'.");

		//load love
		love.Add(currentName + " is one of my best friends!");
		love.Add(currentName + " is such a wonderful goblin");
		love.Add(currentName + " is so cool!");
		love.Add("I fucking love " + currentName + ".");
		love.Add("Me likey " + currentName);
		love.Add(currentName + " is a bad person.  Good Goblin, though.");
		love.Add(currentName + ".  A pretty dope goblin.");
		love.Add(currentName + " and I were pretty tight.");
		love.Add(currentName + " is pretty rad.");
		love.Add(currentName + " is an awesome gob.");
		love.Add("I love " + currentName + " like I love pancakes.  And that's a lot.");
		love.Add("*sigh*  Ahhh " + currentName + "...so dreamy.");
		love.Add(currentName + " haz killer abs, bro.");
		love.Add(currentName + " let me use their toilet after I went to Taco Hell once.  What a swell dude.");
		love.Add(currentName + " and I definitely didn't have a secret love affair last November...");
		love.Add(currentName + " and I might have... run away to Las Vegas and gotten married for a day when we were drunk.  It was a great time.");
		love.Add(currentName + " is a goblin who really knows where their towel is.");


		//load neutral
		neutral.Add(currentName + " and I never really hung out.");
		neutral.Add(currentName + " never gave me any trouble.");
		neutral.Add(currentName + " seems ok, I guess.");
		neutral.Add("I don't really know " + currentName + " very well.");
		neutral.Add("Meh.  " + currentName + ".  Whatever.");
		neutral.Add(currentName + "...  wait, which one was that?");
		neutral.Add(currentName + "?  Just 'cause the live here don't mean I know 'em.");
		neutral.Add(currentName + " and I are just acquaintances.");

		//load lists into dictionary
		DialogueDict["hate"] = hate;
		DialogueDict["love"] = love;
		DialogueDict["neutral"] = neutral;

		int randomDialogueIndex = Random.Range(0, DialogueDict[feel].Count);	//get random index in range of correct set of dialogue options
		List<string> currentList = DialogueDict[feel];							//get current list
		return currentList[randomDialogueIndex];								//return appropriate dialogue				
	}


	public string GetFeeling()
	{
		string feeling = "";
		int odds = Random.Range(1, 100);		//get random number between 1 and 100						
		if ((odds >= 1) && (odds <= 33))		//if odds = between 1 and 10
		{
			feeling = "hate";
		}
		else if ((odds >= 34) && (odds <= 66))	//if odds = between 11 and 55
		{
			feeling = "love";
		}
		else									//if odds = above 55
		{
			feeling = "neutral";
		}
		return feeling;
	}


	public string GetName()
	{
		//get random name, remove it from possible names
		int randomNameIndex = Random.Range(0, names.Count - 1);
		string randomName = names[randomNameIndex];
		names.RemoveAt(randomNameIndex);

		int randomTitleIndex = Random.Range(0, titles.Count - 1);
		string randomTitle = titles[randomTitleIndex];
		titles.RemoveAt(randomTitleIndex);

		string wholeName = (randomName + " the " + randomTitle);

		return wholeName;
	}


	public string GetWeapon(int weaponIndex)
	{
		return weapons[weaponIndex];
	}

	//called by GameSetup
	public void LoadNames()
	{
		names.Add("Adolf");
		names.Add("Alabaster");
		names.Add("Alex");
		names.Add("Altmer");
		names.Add("Anton");

		names.Add("Barbar");
		names.Add("Barrack");
		names.Add("Barry");
		names.Add("Bartholomew");
		names.Add("Beatrix");
		names.Add("Bem");
		names.Add("Benjamin");
		names.Add("Brick");
		names.Add("Brock");

		names.Add("Cole");

		names.Add("Danny");
		names.Add("Dash");
		names.Add("Dave");
		names.Add("Donald");
		names.Add("Drake");

		names.Add("Galorf");
		names.Add("Gat");
		names.Add("Gertrude");
		names.Add("Geezebox");
		names.Add("Gizoko");
		names.Add("Godrich");
		names.Add("Gojira");
		names.Add("Gorb");
		names.Add("Grog");
		names.Add("Grognak");
		names.Add("Grunkel");
		names.Add("Gizebox");

		names.Add("Haru");
		names.Add("Hellena");
		names.Add("Herando");
		names.Add("Hillary");
		names.Add("Homer");
		titles.Add("Horace");

		names.Add("Ichabod");

		names.Add("Jackson");
		names.Add("Jaxkika");
		names.Add("Jen");
		names.Add("Jesus");
		names.Add("Joey");
		names.Add("Jorge");
		names.Add("Justin");

		names.Add("Kanye");
		names.Add("Keylith");
		names.Add("Kirk");
		names.Add("Kojima");
		names.Add("Krog");

		names.Add("Larry");
		names.Add("Lenny");
		names.Add("Leon");
		names.Add("Leopold");
		names.Add("Liam");
		names.Add("Lou");
		names.Add("Louie");

		names.Add("Matthew");
		names.Add("Michael");
		names.Add("Mike");
		names.Add("Mo");

		names.Add("Nilbog");
		names.Add("Nizmod");

		names.Add("Octavious");
		names.Add("Orion");

		names.Add("Palb");
		names.Add("Percy");
		names.Add("Pike");
		names.Add("Preston");

		names.Add("Raf");
		names.Add("Richard");
		names.Add("Robob");
		names.Add("Rosie");
		names.Add("Russel");
		names.Add("Ryan");

		names.Add("Sam");
		names.Add("Samson");
		names.Add("Scamos");
		names.Add("Scanlan");
		names.Add("Scar");
		names.Add("Schnizm");
		names.Add("Skodibo");
		names.Add("Steve");
		names.Add("Smelbog");
		names.Add("Smith");

		names.Add("Tar");
		names.Add("Timmy");
		names.Add("Tunx");

		names.Add("Vax");
		names.Add("Veluro");
		names.Add("Vex");
		names.Add("Vix");
		names.Add("Vox");
		names.Add("Vux");

		names.Add("Xander");
		names.Add("Xeres");

		names.Add("Wayne");
		names.Add("Willis");

		names.Add("Zip");
	}

	//Called by GameSetup
	public void LoadTitles()
	{
		titles.Add("Amazing");
		titles.Add("Awesome");

		titles.Add("Beautiful");
		titles.Add("Bedwetter");
		titles.Add("Blasphemous");
		titles.Add("Bodacious");
		titles.Add("Bomb");
		titles.Add("Bold");
		titles.Add("Bootylicious");
		titles.Add("Bug-Eyed");
		titles.Add("Butthurt");

		titles.Add("Carefree");
		titles.Add("Challenged");
		titles.Add("Charming");
		titles.Add("Chode");
		titles.Add("Combustible");
		titles.Add("Complex");
		titles.Add("Complicated");
		titles.Add("Cranky");
		titles.Add("Crazy");
		titles.Add("Creative");
		titles.Add("Crustacean");
		titles.Add("Crusty");
		titles.Add("Cute");

		titles.Add("Dank");
		titles.Add("Darned");
		titles.Add("Derpy");
		titles.Add("Destroyer");
		titles.Add("Dimwitted");
		titles.Add("Dingus");
		titles.Add("Dope");
		titles.Add("Drug-Addled");
		titles.Add("Drunk");
		titles.Add("Dungeon Master");
		titles.Add("Dusty");

		titles.Add("Easy");
		titles.Add("Emo");
		titles.Add("Emotional");
		titles.Add("Erotic");
		titles.Add("Ethnic");
		titles.Add("Extreme");

		titles.Add("Flashy");
		titles.Add("Flat-Footed");
		titles.Add("Flatulent");
		titles.Add("Fleek");
		titles.Add("Fluffy");
		titles.Add("Forgetful");

		titles.Add("Gaudy");
		titles.Add("Gendered");
		titles.Add("Gobbler");
		titles.Add("Gooey");
		titles.Add("Goth");
		titles.Add("Greedy");
		titles.Add("Grotesque");
		titles.Add("Grouchy");
		titles.Add("Grumbley");

		titles.Add("Handicapped");
		titles.Add("Hard");
		titles.Add("Heavy");
		titles.Add("Heroic");
		titles.Add("Hobbled");
		titles.Add("Human Lover");
		titles.Add("Hung");
		titles.Add("Hung Over");

		
		titles.Add("Impatient");
		titles.Add("Incompetent");
		titles.Add("Incontinent");
		titles.Add("Insane");
		titles.Add("Insolent");
		titles.Add("Isometric");

		titles.Add("Kingly");
		titles.Add("Kleptomaniac");

		titles.Add("Lanky");
		titles.Add("Laughy");
		titles.Add("Lazy");
		titles.Add("Lethargic");
		titles.Add("Light");
		titles.Add("Lightheaded");
		titles.Add("Limpy");
		titles.Add("Lippy");
		titles.Add("Lit");
		titles.Add("Loopy");
		titles.Add("Loud");

		titles.Add("Macho");
		titles.Add("Maniac");
		titles.Add("Marvelous");
		titles.Add("Medium");
		titles.Add("Mumbler");
		titles.Add("Musty");

		titles.Add("Nasty");
		titles.Add("Nincompoop");

		titles.Add("Ogre");
		titles.Add("Ogress");
		titles.Add("Oppressed");
		titles.Add("Oppressor");
		titles.Add("Ornery");

		titles.Add("Parched");
		titles.Add("Patient");
		titles.Add("Perterbed");
		titles.Add("Perverse");
		titles.Add("Prety");
		titles.Add("Problematic");
		titles.Add("Proper");
		titles.Add("Proud");
		titles.Add("Purty");
		titles.Add("Putrid");

		titles.Add("Qualified");
		titles.Add("Queen");
		titles.Add("Quilter");

		titles.Add("Racist");
		titles.Add("Righteous");
		titles.Add("Risky");
		titles.Add("Risque");
		titles.Add("Rough");
		titles.Add("Rowdy");

		titles.Add("Salty");
		titles.Add("Scandalous");
		titles.Add("Seamless");
		titles.Add("Shady");
		titles.Add("Simple");
		titles.Add("Sincere");
		titles.Add("Slim");
		titles.Add("Slippery");
		titles.Add("Sloth");
		titles.Add("Smelly");
		titles.Add("Sodomite");
		titles.Add("Spontaneous");
		titles.Add("Squishy");
		titles.Add("Stanky");
		titles.Add("Steamy");
		titles.Add("Stickey");
		titles.Add("Stoned");
		titles.Add("Stoneheaded");
		titles.Add("Stupdendous");
		titles.Add("Swanky");

		titles.Add("Tasty");
		titles.Add("Top-Heavy");
		titles.Add("Traveler");
		titles.Add("Tubular");
		titles.Add("Tyrant");

		titles.Add("Unbelievable");
		titles.Add("Unqualified");

		titles.Add("Vegan");
		titles.Add("Vegetarian");

		titles.Add("Warrior");
		titles.Add("Wary");
		titles.Add("Wimpy");
		titles.Add("Wonderful");
		titles.Add("Wretched");

		titles.Add("Xenophobe");
	}


	//called by GameSetup
	public void LoadWeapons()
	{
		weapons.Add("sword");
		weapons.Add("axe");
		weapons.Add("dagger");
		weapons.Add("spear");
		weapons.Add("brick");
		weapons.Add("club");
	}
}
