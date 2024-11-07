using _Project.Data;
using UnityEngine;

namespace _Project.Particle_Effects
{
    public static class ParticleColorChanger
    {
        public static Color GetElementColor(ElementData.Element element)
        {
            return element switch
            {
                ElementData.Element.Fire => Color.red,
                ElementData.Element.Earth => Color.green,
                ElementData.Element.Water => Color.blue,
                ElementData.Element.Wind => Color.cyan,
                _ => Color.white
            };
        }

        public static void SetParticleElementColor(ParticleSystem particleSystem, ElementData.Element element)
        {
            if (particleSystem == null) return;
            
            var particleMain = particleSystem.main;
            particleMain.startColor = GetElementColor(element);
        }
    }
}