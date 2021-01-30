using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyIsland.Characters
{
    public class Enemy : Character
    {
        private void Update()
        {
            if (health <= 0) Died();
        }

        private void Died()
        {
            Destroy(gameObject);
        }
    }
}
