using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuppkg
{
    class Arguments
    {

        private static Dictionary<String, Arguments> ARGS;

        public string name;
        public String[] identifier;

        static Arguments()
        {
            ARGS = new Dictionary<String, Arguments>();
        }

        public Arguments(string name, string alias, string alias2)
        {
            this.name = name;
            identifier = new String[] { alias, alias2 };

            _add();
        }

        public Arguments(string name, string alias)
        {
            this.name = name;
            identifier = new String[] { alias };

            _add();
        }

        private void _add() {
            ARGS.Add(name, this);
        }

        public static Arguments GetArgsByName(string name) {
            return ARGS[name];
        }

        public static Arguments CheckArgsFromIdentifier(string input) {
            foreach(KeyValuePair<string, Arguments> entry in ARGS)
            {      
                for(int i = 0; i < entry.Value.identifier.Length; i++)
                {
                    bool result = (input == entry.Value.identifier[i]);
                    if (result) return entry.Value;
                }
            }

            return null;
        }
    }
}
