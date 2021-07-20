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

    public int endGameWasteCount = 0;

    public List<Sprite> achievementSprites;

    #region Requirements
    private const int JUMPING_JACK = 50;
    private const int SUPER_CHUGGER = 10;
    private const int STANDUP_CITIZEN = 10;
    private const float DAREDEVIL = 30.0f; // 30
    // A stack of bottles is a "bottle tower" if it is at least BOTTLE_TOWER tall.
    private const int BOTTLE_TOWER = 10; // 10
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
            new Achievement(0, "Aficionado", "Launch our game.", "Only true gaming and soda connoisseurs would pick our game to play from the bunch. Cheers!", achievementSprites[0], true),
            new Achievement(1, "Socialite", "Complete the tutorial.", "It's a dangerous business, going out your door. You step onto the road, and if you don't keep your feet, there's no knowing where you might be swept off to. - Bilbo Baggins", achievementSprites[1]),
            new Achievement(2, "Jumping Jack", "Jump " + JUMPING_JACK + " times.", "Lots of jumping after drinking all that soda? Thank god the devs didn't code a way to feel sick.", achievementSprites[2]),
            new Achievement(3, "Super Chugger", "Drink " + SUPER_CHUGGER + " times.", "Not one bathroom break needed: the sign of a true king.", achievementSprites[3]),
            new Achievement(4, "Standup Citizen", "Recycle " + STANDUP_CITIZEN + " times.", "The council still has every right to harass you but you'll still take the moral high ground.", achievementSprites[4]),
            new Achievement(5, "Daredevil", "Fall more than " + DAREDEVIL + " feet.", "I'm not seeking penance for what I've done, Father. I'm asking forgiveness, for what I'm about to do. - Matt Murdock", achievementSprites[5]),
            //new Achievement(6, "Jenga Master", "Remove a bottle from a bottle tower.", "Jenga mastery will make you look cool at some parties, just not cool parties."),
            new Achievement(6, "Questionable Architect", "Build a bottle tower.", "Architecture is a very dangerous job. If a writer makes a bad book, eh, people don't read it. But if you make bad architecture, you impose ugliness on a place for a hundred years. - Renzo Piano", achievementSprites[6]),
            new Achievement(7, "Pavement Pounder", "Complete Level 1.", "Meticulously curated Spotify playlist on, you're iPod shuffling down the street. You love to WALKMAN.", achievementSprites[7]),
            new Achievement(8, "Unlimited Power", "Complete the game.", "Wow, you really put up with this game for that long. Thanks, I guess.", achievementSprites[8]),
            new Achievement(9, "Carbon Cutter", "Recycle more than you dump.", "Cleanliness is next to godliness - you're a saint for leaving the world better than you found it.", achievementSprites[9]),

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
            { achievements[9].title.ToLower(), achievements[9] }
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

    public void UpdateLongestFallDistance(float longestFallDistance_)
    {
        longestFallDistance = longestFallDistance_;
        if (longestFallDistance >= DAREDEVIL && !achievementsDic["daredevil"].achieved)
        {
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

        /*if (bottleStack == BOTTLE_TOWER && !addingToStack && !achievementsDic["jenga master"].achieved) {
            achievementsDic["jenga master"].achieved = true;

            OnAchievementUnlockedEventArgs e = new OnAchievementUnlockedEventArgs
            {
                title = achievementsDic["jenga master"].title,
                requirement = achievementsDic["jenga master"].requirement,
                sprite = achievementsDic["jenga master"].sprite
            };
            OnAchievementUnlocked?.Invoke(this, e);
        }*/

        if (bottleStack >= BOTTLE_TOWER && !achievementsDic["questionable architect"].achieved)
        {
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
