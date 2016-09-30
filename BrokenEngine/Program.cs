using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrokenEngine
{

    // T O  D O ' S:
    // TODO: Component Events: oninit, onawake, onstart
    // TODO: Load textures to gpu
    // TODO: Shade using Normal map
    // TODO: create timemanager, which keeps track of time since startup etc
    // TODO: gpu instancing
    // TODO: shader hotreloading
    // TODO: xml scene loading
    // TODO: prefab system
    // TODO: better toon shading (http://prideout.net/blog/?p=22)

    // BUGS:
    // none :o

    class Program
    {

        private static Engine engine;
        
        static void Main(string[] args)
        {
            var wrapper = new WindowsEngineWrapper();
            wrapper.Configure("My Test Game");
            engine = wrapper.Engine;
            wrapper.Run();
        }
        
    }
}
