using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	[System.Serializable]
	public struct LatLonColumn {

		public float height;
		public Vector2 postion;

	}

	public class DotNetSucksBalls {

        public static bool ConsistsOfWhiteSpace(string s){
            foreach(char c in s){
                if(c != ' ') return false;
            }
            return true;
        
        }

        public static bool IsNullOrWhiteSpace(string s) {
            return s == null || ConsistsOfWhiteSpace(s);
        }

    }

    public class ZipCode {

        public string id, name;
        public float population;
        public float area;

        public List<Vector2> points = new List<Vector2>();
        public Mesh mesh = null;

        public ZipCode(string id, string name, float pop, float area) {

            this.id = id;
            this.name = name;

            this.population = pop;
            this.area = area;

        }

    }
    
    public struct Tri {

        public int a, b, c;

        public Tri(int a, int b, int c) {
            
            this.a = a;
            this.b = b;
            this.c = c;

        }

    }

}
