

public class HealthEXPSystem


{
    private int hp;         //health point
    private int exp;        //experience


    public HealthEXPSystem(int hp, int exp) {

        this.hp = hp;
        this.exp = exp;
    }



    public int getHP() {
        return hp;
    }

    public void editHP(int value) {

        hp += value;
    }


    public int getEXP()
    {
        return exp;
    }

    public void editEXP(int value)
    {

        exp += value;
    }
}
