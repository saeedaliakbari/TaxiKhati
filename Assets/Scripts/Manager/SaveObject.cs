using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{//آبجکت ذخیره سازی شامل لیست ماشین ها و لیست باکس های جایزه ای
    public List<CarObject> listCars = new List<CarObject>();
    public List<BoxObject> listBoxes = new List<BoxObject>();
}

[System.Serializable]
public class CarObject
{
    public int level;
    public bool driving;
    public int parkingIndex;
}

[System.Serializable]
public class BoxObject
{
    public int carLevel;
    public int parkingIndex;
}
