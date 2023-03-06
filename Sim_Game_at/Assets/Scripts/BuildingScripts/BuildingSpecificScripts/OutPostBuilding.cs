using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPostBuilding : MonoBehaviour, IAgentInteractions, ITimeTickers, IBuildingActions
{

    private BuildingIdentifier buildingId;

    // Start is called before the first frame update
    void Start()
    {
        buildingId = transform.GetComponent<BuildingIdentifier>();
        buildingId.agentActions = this;
        buildingId.buildingActions = this;
        buildingId.buildingTimer = this;
        Init();
    }

    private void Init() 
    {

        GeneralUtil.dataBank.RecalcAllAllowedTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteBuilding()
    {

    }


    public void LandedOnEntrance(AgentData data)
    {
        Destroy(data.agentObj);
        data.readyToWork = true;
        data.atWork = true;

        if (GeneralUtil.timeCycle.isNightTime)
        {
            if (data.refToHouse != null)
            {
                if (data.readyToWork == true && data.currAction == AgentData.CURRENT_ACTION.WORKING)
                {
                    data.SetAgentPathing(buildingId.buildingData.entrancePoints[0], data.refToHouse.entrancePoints[0], true);
                    GeneralUtil.map.SpawnAgent(data.guid, GeneralUtil.map.tilesArray[buildingId.buildingData.entrancePoints[0].x, buildingId.buildingData.entrancePoints[0].y]);
                    data.readyToWork = false;
                    data.atWork = false;
                }
            }
        }
    }

    public void DayTick()
    {
    }

    public void HourTick()
    { 
    }

    public void MinuteTick()
    {
        LookForWorkers();
    }

    private void LookForWorkers()
    {
        if (buildingId.buildingData.workers.Count < buildingId.buildingData.maxWorkers)
        {
            if (GeneralUtil.dataBank.unemployedNpc.Count > 0)
            {
                for (int i = GeneralUtil.dataBank.unemployedNpc.Count; i-- > 0;)
                {
                    buildingId.AddWorker(GeneralUtil.dataBank.unemployedNpc[i]);
                }
            }
        }
    }

}

