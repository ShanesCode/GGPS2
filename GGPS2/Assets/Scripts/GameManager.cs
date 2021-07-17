using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int spawnRoom;

    public bool tutorialComplete = false;
    private int jumpCount = 0;
    private int drinkCount = 0;
    private int wasteCount = 0;
    private int recycleCount = 0;

    public float longestFallDistance = 0.0f;
    public int bottleStack = 0;
    public bool level1Complete = false;
    public bool gameComplete = false;

    #region Requirements
    private const int JUMPING_JACK = 250;
    private const int SUPER_CHUGGER = 50;
    private const int STANDUP_CITIZEN = 1; // 10
    private const float DAREDEVIL = 3.0f; // 30
    // A stack of bottles is a "bottle tower" if it is at least BOTTLE_TOWER tall.
    private const int BOTTLE_TOWER = 2; // 10
    #endregion

    public List<Achievement> achievements;

    public Dictionary<string, Achievement> achievementsDic;

    public event EventHandler<OnAchievementUnlockedEventArgs> OnAchievementUnlocked;
    public class OnAchievementUnlockedEventArgs
    {
        public string title;
        public string requirement;
        public Sprite sprite;
    }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this)
        {
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }

        achievements = new List<Achievement>()
        {
            new Achievement(0, "Aficionado", "Launch our game.", "Only true gaming and soda connoisseurs would pick our game to play from the bunch. Cheers!", null, true),
            new Achievement(1, "Socialite", "Complete the tutorial.", "It's a dangerous business, going out your door. You step onto the road, and if you don't keep your feet, there's no knowing where you might be swept off to. - Bilbo Baggins"),
            new Achievement(2, "Jumping Jack", "Jump " + JUMPING_JACK + " times.", "Lots of jumping after drinking all that soda? Thank god the devs didn't code a way to feel sick."),
            new Achievement(3, "Super Chugger", "Drink " + SUPER_CHUGGER + " times.", "Not one bathroom break needed: the sign of a true king."),
            new Achievement(4, "Standup Citizen", "Recycle " + STANDUP_CITIZEN + " times.", "The council still has every right to harass you but you'll still take the moral high ground."),
            new Achievement(5, "Daredevil", "Fall more than " + DAREDEVIL + " feet.", "I'm not seeking penance for what I've done, Father. I'm asking forgiveness, for what I'm about to do. - Matt Murdock"),
            new Achievement(6, "Jenga Master", "Remove a bottle from a bottle tower.", "Jenga mastery will make you look cool at some parties, just not cool parties."),
            new Achievement(7, "Questionable Architect", "Build a bottle tower.", "Architecture is a very dangerous job. If a writer makes a bad book, eh, people don't read it. But if you make bad architecture, you impose ugliness on a place for a hundred years. - Renzo Piano"),
            new Achievement(8, "Pavement Pounder", "Complete Level 1.", "Meticulously curated Spotify playlist on, you're iPod shuffling down the street. You love to WALKMAN."),
            new Achievement(9, "Unlimited Power", "Complete the game.", "Wow, you really put up with this game for that long. Thanks, I guess."),
            new Achievement(10, "Carbon Cutter", "Recycle more than you dump.", "Cleanliness is next to godliness - you're a saint for leaving the world better than you found it."),
            new Achievement(11, "11"),
            new Achievement(12, "12"),
            new Achievement(13, "13"),
            new Achievement(14, "14"),
            new Achievement(15, "15"),
            new Achievement(16, "16"),
            new Achievement(17, "17"),
            new Achievement(18, "18"),
            new Achievement(19, "19")
        };

            achievementsDic = new Dictionary<string, Achievement>()
        {
            { achievements[0].title.ToLower(), achievements[0] },
            { achievements[1].title.ToLower(), achievements[1] },
            { achievements[2].title.ToLower(), achievements[2] },
            { achievements[3].title.ToLower(), achievements[3] },
            { achievements[4].title.ToLower(), achievements[4] },
            { achievements[5].title.ToLower(), achievements[5] },
            { achievements[6].title.ToLower(), achievements[6] },
            { achievements[7].title.ToLower(), achievements[7] },
            { achievements[8].title.ToLower(), achievements[8] },
            { achievements[9].title.ToLower(), achievements[9] },
            { achievements[10].title.ToLower(), achievements[10] },
            { achievements[11].title.ToLower(), achievements[11] },
            { achievements[12].title.ToLower(), achievements[12] },
            { achievements[13].title.ToLower(), achievements[13] },
            { achievements[14].title.ToLower(), achievements[14] },
            { achievements[15].title.ToLower(), achievements[15] },
            { achievements[16].title.ToLower(), achievements[16] },
            { achievements[17].title.ToLower(), achievements[17] },
            { achievements[18].title.ToLower(), achievements[18] },
            { achievements[19].title.ToLower(), achievements[19] },
        };
    }

    public void SetSpawnRoom(int roomNumber)
    {
        spawnRoom = roomNumber;
    }

    public int GetDrinkCount()
    {
        return drinkCount;
    }

    public int GetJumpCount()
    {
        return jumpCount;
    }

    public int GetWasteCount()
    {
        return wasteCount;
    }

    public int GetRecycleCount()
    {
        return recycleCount;
    }

    public void UpdateTutorialComplete(bool tutorialComplete_)
    {
        tutorialComplete = tutorialComplete_;
        if (tutorialComplete && !achievementsDic["socialite"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["socialite"].title);
            achievementsDic["socialite"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["socialite"].title,
                requirement = achievementsDic["socialite"].requirement,
                sprite = achievementsDic["socialite"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateJumpCount(int jumpCount_)
    {
        jumpCount = jumpCount_;
        if (jumpCount == JUMPING_JACK && !achievementsDic["jumping jack"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["jumping jack"].title);
            achievementsDic["jumping jack"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["jumping jack"].title,
                requirement = achievementsDic["jumping jack"].requirement,
                sprite = achievementsDic["jumping jack"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateDrinkCount(int drinkCount_)
    {
        drinkCount = drinkCount_;
        if (drinkCount == SUPER_CHUGGER && !achievementsDic["super chugger"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["super chugger"].title);
            achievementsDic["super chugger"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["super chugger"].title,
                requirement = achievementsDic["super chugger"].requirement,
                sprite = achievementsDic["super chugger"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateWasteCount(int wasteCount_)
    {
        wasteCount = wasteCount_;
        /*if (drinkCount == SUPER_CHUGGER && !achievementsDic["super chugger"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["super chugger"].title);
            achievementsDic["super chugger"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["super chugger"].title,
                requirement = achievementsDic["super chugger"].requirement,
                sprite = achievementsDic["super chugger"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }*/
    }

    public void UpdateRecycleCount(int recycleCount_)
    {
        recycleCount = recycleCount_;
        if (recycleCount == STANDUP_CITIZEN && !achievementsDic["standup citizen"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["standup citizen"].title);
            achievementsDic["standup citizen"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["standup citizen"].title,
                requirement = achievementsDic["standup citizen"].requirement,
                sprite = achievementsDic["standup citizen"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateLongestFallDistance(int longestFallDistance_)
    {
        longestFallDistance = longestFallDistance_;
        if (longestFallDistance == DAREDEVIL && !achievementsDic["daredevil"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["daredevil"].title);
            achievementsDic["daredevil"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["daredevil"].title,
                requirement = achievementsDic["daredevil"].requirement,
                sprite = achievementsDic["daredevil"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateBottleStack(int bottleStack_, bool addingToStack)
    {
        bottleStack = bottleStack_;

        if (bottleStack == BOTTLE_TOWER && !addingToStack && !achievementsDic["jenga master"].achieved) {
            Debug.Log("Achievement unlocked: " + achievementsDic["jenga master"].title);
            achievementsDic["jenga master"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["jenga master"].title,
                requirement = achievementsDic["jenga master"].requirement,
                sprite = achievementsDic["jenga master"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }

        if (bottleStack == (BOTTLE_TOWER - 1) && addingToStack && !achievementsDic["questionable architect"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["questionable architect"].title);
            achievementsDic["questionable architect"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["questionable architect"].title,
                requirement = achievementsDic["questionable architect"].requirement,
                sprite = achievementsDic["questionable architect"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
        
    }

    public void UpdateLevel1Complete(bool level1Complete_)
    {
        level1Complete = level1Complete_;
        if (level1Complete && !achievementsDic["pavement pounder"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["pavement pounder"].title);
            achievementsDic["pavement pounder"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["pavement pounder"].title,
                requirement = achievementsDic["pavement pounder"].requirement,
                sprite = achievementsDic["pavement pounder"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UpdateGameComplete(bool gameComplete_)
    {
        gameComplete = gameComplete_;
        if (gameComplete && !achievementsDic["unlimited power"].achieved)
        {
            Debug.Log("Achievement unlocked: " + achievementsDic["unlimited power"].title);
            achievementsDic["unlimited power"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["unlimited power"].title,
                requirement = achievementsDic["unlimited power"].requirement,
                sprite = achievementsDic["unlimited power"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }
    }

    public void UnlockAchievement(string achievementTitle)
    {
        string achTitle = achievementTitle.ToLower();
        Debug.Log("Achievement unlocked: " + achievementsDic[achTitle].title);
        achievementsDic[achTitle].achieved = true;

        OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
        {
            title = achievementsDic[achTitle].title,
            requirement = achievementsDic[achTitle].requirement,
            sprite = achievementsDic[achTitle].sprite
        };
        OnAchievementUnlocked?.Invoke(this, e);
    }
}
