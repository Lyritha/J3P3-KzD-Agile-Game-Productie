using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventsConfig", menuName = "Events/EventsConfig")]
public class EventsConfig : ScriptableObject
{
    public List<Event> achterhoekEvents = new List<Event>();
    public List<Event> pheonixEvents = new List<Event>();
    public List<Event> amerikaEvents = new List<Event>();
}
