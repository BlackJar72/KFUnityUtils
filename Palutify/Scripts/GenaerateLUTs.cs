using UnityEngine;


namespace kfutils.graphics.palutify {


    public class GenerateLUTs : MonoBehaviour
    {
        const int ROWS = 8;
        const int COLOR_INT = ROWS * ROWS;
        const int COLOR_INT_SQ = COLOR_INT * COLOR_INT;
        const int LUT_PIXELS = COLOR_INT * COLOR_INT * COLOR_INT;
        const float COLOR_SCALE = (float)COLOR_INT;

        [SerializeField] Texture2D palette;

        private ColorRegistrar registrar = new();
        private int r, g, b, x, y, counter;
        private bool processing = false, done = false;
        Color current = new(0, 0, 0, 1);
        [SerializeField] public Texture2D lut;

        private delegate void ProcessPixel(GenerateLUTs test);
        private ProcessPixel PixelProcessor = ProcessPixelRGB;


        void Awake()
        {
            Start();
        }


        void Start()
        {
            if(palette == null) return;
            registrar.PopulateFromTexture(palette);
            Debug.Log("Found " + registrar.ColorCount + " colors in palette texture " + palette.name);
            Debug.Log(palette.format);
            Debug.Log(lut.format);
            Debug.Log(lut.dimension);
            Debug.Log(lut.GetPixels().Length);
            MakeLUT();
        }


        void Update()
        {
            if(processing)
            {
                if(counter < LUT_PIXELS) {
                    for(int i = 0; i < COLOR_INT; i++) PixelProcessor(this);
                    lut.Apply();
                }
                else
                {
                    processing = false;
                    lut.SaveAsset(); 
                    done = true;
                }
            }
            if(done) Application.Quit();

        }


        private void MakeLUT()
        {
            counter = 0;
            processing = true;
        }


        private static void ProcessPixelHSV(GenerateLUTs tests)
        {
            tests.r = tests.counter % COLOR_INT;
            tests.g = (tests.counter / COLOR_INT) % COLOR_INT;
            tests.b = tests.counter / COLOR_INT_SQ;
            {
                tests.x = tests.r + ((tests.b % ROWS) * COLOR_INT);
                tests.y = tests.g + ((tests.b / ROWS) * COLOR_INT);
                tests.current.r = (float)tests.r / COLOR_SCALE;
                tests.current.g = (float)tests.g / COLOR_SCALE;
                tests.current.b = (float)tests.b / COLOR_SCALE;
                tests.lut.SetPixel(tests.x, tests.y, tests.registrar.FindClosestRegisteredHSV(tests.current));   
            }
            tests.counter++;
        }


        private static void ProcessPixelRGB(GenerateLUTs tests)
        {
            tests.r = tests.counter % COLOR_INT;
            tests.g = (tests.counter / COLOR_INT) % COLOR_INT;
            tests.b = tests.counter / COLOR_INT_SQ;
            {
                tests.x = tests.r + ((tests.b % ROWS) * COLOR_INT);
                tests.y = tests.g + ((tests.b / ROWS) * COLOR_INT);
                tests.current.r = (float)tests.r / COLOR_SCALE;
                tests.current.g = (float)tests.g / COLOR_SCALE;
                tests.current.b = (float)tests.b / COLOR_SCALE;
                tests.lut.SetPixel(tests.x, tests.y, tests.registrar.FindClosestRegisteredRGB(tests.current));   
            }
            tests.counter++;
        }


        private static void ProcessPixelHRGB(GenerateLUTs tests)
        {
            tests.r = tests.counter % COLOR_INT;
            tests.g = (tests.counter / COLOR_INT) % COLOR_INT;
            tests.b = tests.counter / COLOR_INT_SQ;
            {
                tests.x = tests.r + ((tests.b % ROWS) * COLOR_INT);
                tests.y = tests.g + ((tests.b / ROWS) * COLOR_INT);
                tests.current.r = (float)tests.r / COLOR_SCALE;
                tests.current.g = (float)tests.g / COLOR_SCALE;
                tests.current.b = (float)tests.b / COLOR_SCALE;
                tests.lut.SetPixel(tests.x, tests.y, tests.registrar.FindClosestRegisteredHRGB(tests.current));   
            }
            tests.counter++;
        }


        private static void ProcessPixelHybrid(GenerateLUTs tests)
        {
            tests.r = tests.counter % COLOR_INT;
            tests.g = (tests.counter / COLOR_INT) % COLOR_INT;
            tests.b = tests.counter / COLOR_INT_SQ;
            {
                tests.x = tests.r + ((tests.b % ROWS) * COLOR_INT);
                tests.y = tests.g + ((tests.b / ROWS) * COLOR_INT);
                tests.current.r = (float)tests.r / COLOR_SCALE;
                tests.current.g = (float)tests.g / COLOR_SCALE;
                tests.current.b = (float)tests.b / COLOR_SCALE;
                tests.lut.SetPixel(tests.x, tests.y, tests.registrar.FindClosestRegisteredHybrid(tests.current));   
            }
            tests.counter++;
        }


        private static void ProcessPixelNeutral(GenerateLUTs tests)
        {
            tests.r = tests.counter % COLOR_INT;
            tests.g = (tests.counter / COLOR_INT) % COLOR_INT;
            tests.b = tests.counter / COLOR_INT_SQ;
            {
                tests.x = tests.r + ((tests.b % ROWS) * COLOR_INT);
                tests.y = tests.g + ((tests.b / ROWS) * COLOR_INT);
                tests.current.r = (float)tests.r / COLOR_SCALE;
                tests.current.g = (float)tests.g / COLOR_SCALE;
                tests.current.b = (float)tests.b / COLOR_SCALE;
                tests.lut.SetPixel(tests.x, tests.y, tests.current);   
            }
            tests.counter++;
        }


    }


}