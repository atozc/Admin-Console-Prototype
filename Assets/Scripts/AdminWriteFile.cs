using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System.IO;
using System.Text;
using System.Xml;

public class AdminWriteFile : MonoBehaviour
{
    [System.Serializable]
    public class TestSession
    {
        public string name;
        public string dayAndTime;
        public List<TestRecord> records;
    }

    [System.Serializable]
    public class TestRecord
    {
        public float secondsFromStart;
        public string eventName; //Is the .ToString() of TaiserEventTypes below
        public List<string> eventModifiers; // Only one event type has event modifiers right now
    }

    public class InstrumentMgr : MonoBehaviour
    {
        public static InstrumentMgr inst;
        public void Awake()
        {
            inst = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateOrFindTestFolder();
        }

        // Update is called once per frame
        void Update()
        {
                WriteSession();
        }

        public string TestFolder;

        public void CreateOrFindTestFolder()
        {
            try
            {
                TestFolder = System.IO.Path.Combine(Application.persistentDataPath);
                System.IO.Directory.CreateDirectory(TestFolder);
            }
            catch (System.Exception e)
            {
                Debug.Log("Cannot create Test Directory: " + e.ToString());
            }

        }

        //public List<TaiserRecord> records = new List<TaiserRecord>();
        public TestSession session = new TestSession();

        public void AddRecord(string eventName, List<string> modifiers)
        {
            TestRecord record = new TestRecord();
            record.eventName = eventName;
            record.eventModifiers = modifiers;
            record.secondsFromStart = Time.time;// Time.realtimeSinceStartup;
            session.records.Add(record);
        }

        public void AddRecord(string eventName, string modifier = "")
        {
            TestRecord record = new TestRecord();
            record.eventName = eventName;
            List<string> mods = new List<string>();
            mods.Add(modifier);
            record.eventModifiers = mods;
            record.secondsFromStart = Time.time; // Time.realtimeSinceStartup;
            session.records.Add(record);
        }
        //-----------------------------------------------------------------

        //public string csvString;
        IEnumerator WriteToServer()
        {
            XmlDocument map = new XmlDocument();
            map.LoadXml("<level></level>");
            byte[] levelData = Encoding.UTF8.GetBytes(MakeHeaderString() + MakeRecords());
            string fileName = new string(session.name.ToCharArray()); // Path.GetRandomFileName().Substring(0, 8);
            fileName = fileName + ".csv";
            Debug.Log("FileName: " + fileName);

            WWWForm form = new WWWForm();
            Debug.Log("Created new WWW Form");
            form.AddField("action", "level upload");
            form.AddField("file", "file");
            form.AddBinaryData("file", levelData, fileName, "text/csv");
            Debug.Log("Binary data added");
            WWW w = new WWW("https://www.cse.unr.edu/~sushil/taiser/DataLoad.php", form);
            yield return w;

            if (w.error != null)
            {
                Debug.Log("Error: " + w.error);
                Debug.Log(w.text);
            }
            else
            {
                Debug.Log("No errors");
                Debug.Log(w.text);
                if (w.uploadProgress == 1 || w.isDone)
                {
                    yield return new WaitForSeconds(5);
                    Debug.Log("Waited five seconds");
                }
            }
        }

        //-----------------------------------------------------------------
        public static bool isDebug = true;
        public void WriteSession()
        {
            session.dayAndTime = System.DateTime.Now.ToUniversalTime().ToString();
            string tmp = System.DateTime.Now.ToLocalTime().ToString();

            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(TestFolder, session.name + ".csv"), FileMode.Create), Encoding.UTF8))
            {
                WriteHeader(sw);
                WriteRecords(sw);
            }

            StartCoroutine("WriteToServer");
        }

        public void WriteHeader(StreamWriter sw)
        {
            sw.WriteLine(MakeHeaderString());
        }

        string eoln = "\r\n"; //CSV RFC: https://datatracker.ietf.org/doc/html/rfc4180
        public string MakeHeaderString()
        {
            string header = "";
            header += "Packet Speed: ," + session + eoln;
            header += "Time, Event, Modifiers" + eoln;
            return header;
        }

        public void WriteRecords(StreamWriter sw)
        {
            sw.WriteLine(MakeRecords());
        }

        public string MakeRecords()
        {
            string lines = "";
            foreach (TestRecord tr in session.records)
            {
                string mods = CSVString(tr.eventModifiers);
                lines += tr.secondsFromStart.ToString("0000.0") + ", " + tr.eventName + mods + eoln;
            }
            return lines;

        }

        public string CSVString(List<string> mods)
        {
            string modifiers = "";
            foreach (string mod in mods)
            {
                modifiers += ", " + mod;
            }
            return modifiers;
        }

    }
}