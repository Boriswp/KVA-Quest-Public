using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingsTransactor : InteractiveObject
{
    public override void Action()
    {
        KarmaController.Instance.OpenStatue();
    }

}
