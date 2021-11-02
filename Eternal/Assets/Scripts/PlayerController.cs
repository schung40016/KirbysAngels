using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject hp_pack;

    public GameObject exp_pack;

    HealthEXPSystem healthEXP;

    // Start is called before the first frame update
    void Start()
    {
        healthEXP = new HealthEXPSystem(100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //Flaw here, it may destory all the hp pac or exp pack at the end.
        //But it shouldn't be an issue for prfabs
        //To Do: Need to find a way to see what object was touched.

        if (other.name == "Health Point")
        {
            float distance_hp = Vector3.Distance(other.transform.position, this.transform.position);
            if (distance_hp <= 2.5f)
            {


                Debug.Log("HP Before: " + healthEXP.getHP().ToString());
                Debug.Log("HP point got and increase 100 health point");
                healthEXP.addHP(100);
                Debug.Log("HP After: " + healthEXP.getHP().ToString());

                //Destroy(hp_pack);

            }
        }
        else if (other.name == "Exp Point")
        {
            float distance_exp = Vector3.Distance(other.transform.position, this.transform.position);



            if (distance_exp <= 2.5f)
            {


                Debug.Log("EXP Before: " + healthEXP.getEXP().ToString());
                Debug.Log("EXP point got and increase 100 EXP point");
                healthEXP.addEXP(100);
                Debug.Log("EXP After: " + healthEXP.getEXP().ToString());

                //Destroy(exp_pack);


            }
        }





    }


}
