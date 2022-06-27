using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DriverFaunc
{

    public class Focas1
    {
        /* Axis define */
#if FS30D
    public const short MAX_AXIS = 32;
#elif M_AXIS2
    public const short MAX_AXIS = 24;
#elif FS15D
    public const short MAX_AXIS = 10;
#else
        public const short MAX_AXIS = 8;
#endif

        public const short ALL_AXES = (-1);
        public const short ALL_SPINDLES = (-1);
        public const short EW_OK = (short)focas_ret.EW_OK;

        /* Error Codes */
        public enum focas_ret
        {
            EW_PROTOCOL = (-17),           /* protocol error */
            EW_SOCKET = (-16),           /* Windows socket error */
            EW_NODLL = (-15),           /* DLL not exist error */
            EW_BUS = (-11),           /* bus error */
            EW_SYSTEM2 = (-10),           /* system error */
            EW_HSSB = (-9),           /* hssb communication error */
            EW_HANDLE = (-8),           /* Windows library handle error */
            EW_VERSION = (-7),           /* CNC/PMC version missmatch */
            EW_UNEXP = (-6),           /* abnormal error */
            EW_SYSTEM = (-5),           /* system error */
            EW_PARITY = (-4),           /* shared RAM parity error */
            EW_MMCSYS = (-3),           /* emm386 or mmcsys install error */
            EW_RESET = (-2),           /* reset or stop occured error */
            EW_BUSY = (-1),           /* busy error */
            EW_OK = 0,           /* no problem */
            EW_FUNC = 1,           /* command prepare error */
            EW_NOPMC = 1,           /* pmc not exist */
            EW_LENGTH = 2,           /* data block length error */
            EW_NUMBER = 3,           /* data number error */
            EW_RANGE = 3,           /* address range error */
            EW_ATTRIB = 4,           /* data attribute error */
            EW_TYPE = 4,           /* data type error */
            EW_DATA = 5,           /* data error */
            EW_NOOPT = 6,           /* no option error */
            EW_PROT = 7,           /* write protect error */
            EW_OVRFLOW = 8,           /* memory overflow error */
            EW_PARAM = 9,           /* cnc parameter not correct error */
            EW_BUFFER = 10,           /* buffer error */
            EW_PATH = 11,           /* path error */
            EW_MODE = 12,           /* cnc mode error */
            EW_REJECT = 13,           /* execution rejected error */
            EW_DTSRVR = 14,           /* data server error */
            EW_ALARM = 15,           /* alarm has been occurred */
            EW_STOP = 16,           /* CNC is not running */
            EW_PASSWD = 17,           /* protection data error */
            /*
                Result codes of DNC operation
            */
            DNC_NORMAL = (-1),           /* normal completed */
            DNC_CANCEL = (-32768),           /* DNC operation was canceled by CNC */
            DNC_OPENERR = (-514),           /* file open error */
            DNC_NOFILE = (-516),           /* file not found */
            DNC_READERR = (-517)              /* read error */
        };

        /*--------------------*/
        /*                    */
        /* Structure Template */
        /*                    */
        /*--------------------*/
        /*-------------------------------------*/
        /* CNC: Control axis / spindle related */
        /*-------------------------------------*/

        /* cnc_actf:read actual axis feedrate(F) */
        /* cnc_acts:read actual spindle speed(S) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBACT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;      /* dummy */
            public int data;      /* actual feed / actual spindle */
        }

        /* cnc_acts2:read actual spindle speed(S) */
        /* (All or specified ) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBACT2
        {
            public short datano;     /* spindle number */
            public short type;       /* dummy */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] data;       /* spindle data */
        }

        /* cnc_absolute:read absolute axis position */
        /* cnc_machine:read machine axis position */
        /* cnc_relative:read relative axis position */
        /* cnc_distance:read distance to go */
        /* cnc_skip:read skip position */
        /* cnc_srvdelay:read servo delay value */
        /* cnc_accdecdly:read acceleration/deceleration delay value */
        /* cnc_absolute2:read absolute axis position 2 */
        /* cnc_relative2:read relative axis position 2 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAXIS
        {
            public short dummy;  /* dummy */
            public short type;   /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] data;      /* data value */
        }

        /* cnc_rddynamic:read all dynamic data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class FAXIS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] absolute;    /* absolute position */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] machine;     /* machine position */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] relative;    /* relative position */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] distance;    /* distance to go */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OAXIS
        {
            public int absolute;  /* absolute position */
            public int machine;   /* machine position */
            public int relative;  /* relative position */
            public int distance;  /* distance to go */
        }
#if (!ONO8D)
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDY_1
        {
            public short dummy;
            public short axis;      /* axis number */
            public short alarm;     /* alarm status */
            public short prgnum;    /* current program number */
            public short prgmnum;   /* main program number */
            public int seqnum;    /* current sequence number */
            public int actf;      /* actual feedrate */
            public int acts;      /* actual spindle speed */
            public FAXIS pos = new FAXIS();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDY_2
        {
            public short dummy;
            public short axis;      /* axis number */
            public short alarm;     /* alarm status */
            public short prgnum;    /* current program number */
            public short prgmnum;   /* main program number */
            public int seqnum;    /* current sequence number */
            public int actf;      /* actual feedrate */
            public int acts;      /* actual spindle speed */
            public OAXIS pos = new OAXIS();
        }
#else
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public class ODBDY_1
    {
        public short  dummy ;
        public short  axis ;      /* axis number */
        public short  alarm ;     /* alarm status */
        public int    prgnum ;    /* current program number */
        public int    prgmnum ;   /* main program number */
        public int    seqnum ;    /* current sequence number */
        public int    actf ;      /* actual feedrate */
        public int    acts ;      /* actual spindle speed */
        public FAXIS  pos = new FAXIS();
    }
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public class ODBDY_2
    {
        public short  dummy ;
        public short  axis ;      /* axis number */
        public short  alarm ;     /* alarm status */
        public int    prgnum ;    /* current program number */
        public int    prgmnum ;   /* main program number */
        public int    seqnum ;    /* current sequence number */
        public int    actf ;      /* actual feedrate */
        public int    acts ;      /* actual spindle speed */
        public OAXIS  pos = new OAXIS();
    }
#endif

        /* cnc_rddynamic2:read all dynamic data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDY2_1
        {
            public short dummy;
            public short axis;      /* axis number */
            public int alarm;     /* alarm status */
            public int prgnum;    /* current program number */
            public int prgmnum;   /* main program number */
            public int seqnum;    /* current sequence number */
            public int actf;      /* actual feedrate */
            public int acts;      /* actual spindle speed */
            public FAXIS pos = new FAXIS();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDY2_2
        {
            public short dummy;
            public short axis;       /* axis number */
            public int alarm;      /* alarm status */
            public int prgnum;     /* current program number */
            public int prgmnum;    /* main program number */
            public int seqnum;     /* current sequence number */
            public int actf;       /* actual feedrate */
            public int acts;       /* actual spindle speed */
            public OAXIS pos = new OAXIS(); /* In case of 1 axis  */
        }

        /* cnc_wrrelpos:set origin / preset relative axis position */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBWRR
        {
            public short datano; /* dummy */
            public short type;   /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] data = new int[MAX_AXIS];   /* preset data */
        }

        /* cnc_prstwkcd:preset work coordinate */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBWRA
        {
            public short datano; /* dummy */
            public short type;   /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] data = new int[MAX_AXIS];   /* preset data */
        }

        /* cnc_rdmovrlap:read manual overlapped motion value */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBOVL
        {
            public short datano; /* dummy */
            public short type;   /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * MAX_AXIS)]
            public int[] data;   /* data value:[2][MAX_AXIS] */
        }

        /* cnc_rdspload:read load information of serial spindle */
        /* cnc_rdspmaxrpm:read maximum r.p.m. ratio of serial spindle */
        /* cnc_rdspgear:read gear ratio of serial spindle */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPN
        {
            public short datano; /* dummy */
            public short type;   /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] data;   /* preset data */
        }

        /* cnc_rdposition:read tool position */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class POSELM
        {
            public int data;         /* position data */
            public short dec;        /* place of decimal point of position data */
            public short unit;       /* unit of position data */
            public short disp;       /* status of display */
            public char name;        /* axis name */
            public char suff;        /* axis name preffix */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class POSELMALL
        {
            public POSELM abs = new POSELM();
            public POSELM mach = new POSELM();
            public POSELM rel = new POSELM();
            public POSELM dist = new POSELM();
        }

#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBPOS
    {
        public POSELMALL p1=new POSELMALL();
        public POSELMALL p2=new POSELMALL();
        public POSELMALL p3=new POSELMALL();
        public POSELMALL p4=new POSELMALL();
        public POSELMALL p5=new POSELMALL();
        public POSELMALL p6=new POSELMALL();
        public POSELMALL p7=new POSELMALL();
        public POSELMALL p8=new POSELMALL();
        public POSELMALL p9=new POSELMALL();
        public POSELMALL p10=new POSELMALL();
        public POSELMALL p11=new POSELMALL();
        public POSELMALL p12=new POSELMALL();
        public POSELMALL p13=new POSELMALL();
        public POSELMALL p14=new POSELMALL();
        public POSELMALL p15=new POSELMALL();
        public POSELMALL p16=new POSELMALL();
        public POSELMALL p17=new POSELMALL();
        public POSELMALL p18=new POSELMALL();
        public POSELMALL p19=new POSELMALL();
        public POSELMALL p20=new POSELMALL();
        public POSELMALL p21=new POSELMALL();
        public POSELMALL p22=new POSELMALL();
        public POSELMALL p23=new POSELMALL();
        public POSELMALL p24=new POSELMALL();
        // In case of 24 axes.
        // if you need the more information, you must be add the member.
    }
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBPOS
    {
        public POSELMALL p1=new POSELMALL();
        public POSELMALL p2=new POSELMALL();
        public POSELMALL p3=new POSELMALL();
        public POSELMALL p4=new POSELMALL();
        public POSELMALL p5=new POSELMALL();
        public POSELMALL p6=new POSELMALL();
        public POSELMALL p7=new POSELMALL();
        public POSELMALL p8=new POSELMALL();
        public POSELMALL p9=new POSELMALL();
        public POSELMALL p10=new POSELMALL();
        // In case of 10 axes.
        // if you need the more information, you must be add the member.
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPOS
        {
            public POSELMALL p1 = new POSELMALL();
            public POSELMALL p2 = new POSELMALL();
            public POSELMALL p3 = new POSELMALL();
            public POSELMALL p4 = new POSELMALL();
            public POSELMALL p5 = new POSELMALL();
            public POSELMALL p6 = new POSELMALL();
            public POSELMALL p7 = new POSELMALL();
            public POSELMALL p8 = new POSELMALL();
            // In case of 8 axes.
            // if you need the more information, you must be add the member.
        }
#endif

        /* cnc_rdhndintrpt:read handle interruption */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHND_data
        {
            public POSELM input = new POSELM();   /* input unit */
            public POSELM output = new POSELM();  /* output unit */
        }
#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBHND
    {
        public ODBHND_data p1=new ODBHND_data();
        public ODBHND_data p2=new ODBHND_data();
        public ODBHND_data p3=new ODBHND_data();
        public ODBHND_data p4=new ODBHND_data();
        public ODBHND_data p5=new ODBHND_data();
        public ODBHND_data p6=new ODBHND_data();
        public ODBHND_data p7=new ODBHND_data();
        public ODBHND_data p8=new ODBHND_data();
        public ODBHND_data p9=new ODBHND_data();
        public ODBHND_data p10=new ODBHND_data();
        public ODBHND_data p11=new ODBHND_data();
        public ODBHND_data p12=new ODBHND_data();
        public ODBHND_data p13=new ODBHND_data();
        public ODBHND_data p14=new ODBHND_data();
        public ODBHND_data p15=new ODBHND_data();
        public ODBHND_data p16=new ODBHND_data();
        public ODBHND_data p17=new ODBHND_data();
        public ODBHND_data p18=new ODBHND_data();
        public ODBHND_data p19=new ODBHND_data();
        public ODBHND_data p20=new ODBHND_data();
        public ODBHND_data p21=new ODBHND_data();
        public ODBHND_data p22=new ODBHND_data();
        public ODBHND_data p23=new ODBHND_data();
        public ODBHND_data p24=new ODBHND_data();
        // In case of 24 axes.
        // if you need the more information, you must be add the member.
    }
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBHND
    {
        public ODBHND_data p1=new ODBHND_data();
        public ODBHND_data p2=new ODBHND_data();
        public ODBHND_data p3=new ODBHND_data();
        public ODBHND_data p4=new ODBHND_data();
        public ODBHND_data p5=new ODBHND_data();
        public ODBHND_data p6=new ODBHND_data();
        public ODBHND_data p7=new ODBHND_data();
        public ODBHND_data p8=new ODBHND_data();
        public ODBHND_data p9=new ODBHND_data();
        public ODBHND_data p10=new ODBHND_data();
        // In case of 10 axes.
        // if you need the more information, you must be add the member.
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHND
        {
            public ODBHND_data p1 = new ODBHND_data();
            public ODBHND_data p2 = new ODBHND_data();
            public ODBHND_data p3 = new ODBHND_data();
            public ODBHND_data p4 = new ODBHND_data();
            public ODBHND_data p5 = new ODBHND_data();
            public ODBHND_data p6 = new ODBHND_data();
            public ODBHND_data p7 = new ODBHND_data();
            public ODBHND_data p8 = new ODBHND_data();
            // In case of 8 axes.
            // if you need the more information, you must be add the member.
        }
#endif

        /* cnc_rdspeed:read current speed */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class SPEEDELM
        {
            public int data;       /* speed data */
            public short dec;        /* decimal position */
            public short unit;       /* data unit */
            public short disp;       /* display flag */
            public byte name;       /* name of data */
            public byte suff;       /* suffix */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPEED
        {
            public SPEEDELM actf = new SPEEDELM();   /* actual feed rate */
            public SPEEDELM acts = new SPEEDELM();   /* actual spindle speed */
        }

        /* cnc_rdsvmeter:read servo load meter */
        /* cnc_rdspmeter:read spindle load meter */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class LOADELM
        {
            public int data;       /* load meter */
            public short dec;        /* decimal position */
            public short unit;       /* unit */
            public byte name;       /* name of data */
            public byte suff1;      /* suffix */
            public byte suff2;      /* suffix */
            public byte reserve;    /* reserve */
        }

#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBSVLOAD
    {
        public LOADELM svload1 = new LOADELM();     /* servo load meter */
        public LOADELM svload2 = new LOADELM();     /* servo load meter */
        public LOADELM svload3 = new LOADELM();     /* servo load meter */
        public LOADELM svload4 = new LOADELM();     /* servo load meter */
        public LOADELM svload5 = new LOADELM();     /* servo load meter */
        public LOADELM svload6 = new LOADELM();     /* servo load meter */
        public LOADELM svload7 = new LOADELM();     /* servo load meter */
        public LOADELM svload8 = new LOADELM();     /* servo load meter */
        public LOADELM svload9 = new LOADELM();     /* servo load meter */
        public LOADELM svload10= new LOADELM();     /* servo load meter */
        public LOADELM svload11= new LOADELM();     /* servo load meter */
        public LOADELM svload12= new LOADELM();     /* servo load meter */
        public LOADELM svload13= new LOADELM();     /* servo load meter */
        public LOADELM svload14= new LOADELM();     /* servo load meter */
        public LOADELM svload15= new LOADELM();     /* servo load meter */
        public LOADELM svload16= new LOADELM();     /* servo load meter */
        public LOADELM svload17= new LOADELM();     /* servo load meter */
        public LOADELM svload18= new LOADELM();     /* servo load meter */
        public LOADELM svload19= new LOADELM();     /* servo load meter */
        public LOADELM svload20= new LOADELM();     /* servo load meter */
        public LOADELM svload21= new LOADELM();     /* servo load meter */
        public LOADELM svload22= new LOADELM();     /* servo load meter */
        public LOADELM svload23= new LOADELM();     /* servo load meter */
        public LOADELM svload24= new LOADELM();     /* servo load meter */
    }
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBSVLOAD
    {
        public LOADELM svload1 = new LOADELM();     /* servo load meter */
        public LOADELM svload2 = new LOADELM();     /* servo load meter */
        public LOADELM svload3 = new LOADELM();     /* servo load meter */
        public LOADELM svload4 = new LOADELM();     /* servo load meter */
        public LOADELM svload5 = new LOADELM();     /* servo load meter */
        public LOADELM svload6 = new LOADELM();     /* servo load meter */
        public LOADELM svload7 = new LOADELM();     /* servo load meter */
        public LOADELM svload8 = new LOADELM();     /* servo load meter */
        public LOADELM svload9 = new LOADELM();     /* servo load meter */
        public LOADELM svload10= new LOADELM();     /* servo load meter */
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSVLOAD
        {
            public LOADELM svload1 = new LOADELM();     /* servo load meter */
            public LOADELM svload2 = new LOADELM();     /* servo load meter */
            public LOADELM svload3 = new LOADELM();     /* servo load meter */
            public LOADELM svload4 = new LOADELM();     /* servo load meter */
            public LOADELM svload5 = new LOADELM();     /* servo load meter */
            public LOADELM svload6 = new LOADELM();     /* servo load meter */
            public LOADELM svload7 = new LOADELM();     /* servo load meter */
            public LOADELM svload8 = new LOADELM();     /* servo load meter */
        }
#endif

        /* cnc_rdexecpt:read execution program pointer */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class PRGPNT
        {
            public int prog_no;                      /* program number */
            public int blk_no;                       /* block number */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPLOAD_data
        {
            public LOADELM spload = new LOADELM();     /* spindle load meter */
            public LOADELM spspeed = new LOADELM();    /* spindle speed */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPLOAD
        {
            public ODBSPLOAD_data spload1 = new ODBSPLOAD_data();     /* spindle load */
            public ODBSPLOAD_data spload2 = new ODBSPLOAD_data();     /* spindle load */
            public ODBSPLOAD_data spload3 = new ODBSPLOAD_data();     /* spindle load */
            public ODBSPLOAD_data spload4 = new ODBSPLOAD_data();     /* spindle load */
        }

        /* cnc_rd5axmandt:read manual feed for 5-axis machining */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB5AXMAN
        {
            public short type1;
            public short type2;
            public short type3;
            public int data1;
            public int data2;
            public int data3;
            public int c1;
            public int c2;
            public int dummy;
            public int td;
            public int r1;
            public int r2;
            public int vr;
            public int h1;
            public int h2;
        }

        /*----------------------*/
        /* CNC: Program related */
        /*----------------------*/

        /* cnc_rddncdgndt:read the diagnosis data of DNC operation */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDNCDGN
        {
            public short ctrl_word;
            public short can_word;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] nc_file;
            public ushort read_ptr;
            public ushort write_ptr;
            public ushort empty_cnt;
            public uint total_size;
        }

        /* cnc_upload:upload NC program */
        /* cnc_cupload:upload NC program(conditional) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBUP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;  /* dummy */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] data; /* data */
        } /* In case that the number of data is 256 */

        /* cnc_buff:read buffer status for downloading/verification NC program */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBBUF
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;  /* dummy */
            public short data;  /* buffer status */
        }

        /* cnc_rdprogdir:read program directory */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class PRGDIR
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] prg_data; /* directory data */
        } /* In case that the number of data is 256 */

        /* cnc_rdproginfo:read program information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBNC_1
        {
            public short reg_prg;       /* registered program number */
            public short unreg_prg;     /* unregistered program number */
            public int used_mem;      /* used memory area */
            public int unused_mem;    /* unused memory area */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBNC_2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            public char[] asc; /* ASCII string type */
        }

        /* cnc_rdprgnum:read program number under execution */
#if (!ONO8D)
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPRO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;     /* dummy */
            public short data;      /* running program number */
            public short mdata;     /* main program number */
        }
#else
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBPRO
    {
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public short[] dummy ;     /* dummy */
        public int     data ;      /* running program number */
        public int     mdata ;     /* main program number */
    }
#endif

        /* cnc_exeprgname:read program name under execution */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXEPRG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public char[] name;       /* running program name */
            public int o_num;        /* running program number */
        }

        /* cnc_rdseqnum:read sequence number under execution */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSEQ
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;      /* dummy */
            public int data;       /* sequence number */
        }

        /* cnc_rdmdipntr:read execution pointer for MDI operation */
#if (!ONO8D)
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMDIP
        {
            public short mdiprog;    /* exec. program number */
            public int mdipntr;    /* exec. pointer */
            public short crntprog;   /* prepare program number */
            public int crntpntr;   /* prepare pointer */
        }
#else
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBMDIP
    {
        public int     mdiprog;    /* exec. program number */
        public int     mdipntr;    /* exec. pointer */
        public int     crntprog;   /* prepare program number */
        public int     crntpntr;   /* prepare pointer */
    }
#endif

        /* cnc_rdaxisdata:read various axis data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAXDT_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string name = new string(' ', 4);   /* axis name */
            public int data;                       /* position data */
            public short dec;                        /* decimal position */
            public short unit;                       /* data unit */
            public short flag;                       /* flags */
            public short reserve;                    /* reserve */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAXDT
        {
            public ODBAXDT_data data1 = new ODBAXDT_data();
            public ODBAXDT_data data2 = new ODBAXDT_data();
            public ODBAXDT_data data3 = new ODBAXDT_data();
            public ODBAXDT_data data4 = new ODBAXDT_data();
            public ODBAXDT_data data5 = new ODBAXDT_data();
            public ODBAXDT_data data6 = new ODBAXDT_data();
            public ODBAXDT_data data7 = new ODBAXDT_data();
            public ODBAXDT_data data8 = new ODBAXDT_data();
            public ODBAXDT_data data9 = new ODBAXDT_data();
            public ODBAXDT_data data10 = new ODBAXDT_data();
            public ODBAXDT_data data11 = new ODBAXDT_data();
            public ODBAXDT_data data12 = new ODBAXDT_data();
            public ODBAXDT_data data13 = new ODBAXDT_data();
            public ODBAXDT_data data14 = new ODBAXDT_data();
            public ODBAXDT_data data15 = new ODBAXDT_data();
            public ODBAXDT_data data16 = new ODBAXDT_data();
            public ODBAXDT_data data17 = new ODBAXDT_data();
            public ODBAXDT_data data18 = new ODBAXDT_data();
            public ODBAXDT_data data19 = new ODBAXDT_data();
            public ODBAXDT_data data20 = new ODBAXDT_data();
            public ODBAXDT_data data21 = new ODBAXDT_data();
            public ODBAXDT_data data22 = new ODBAXDT_data();
            public ODBAXDT_data data23 = new ODBAXDT_data();
            public ODBAXDT_data data24 = new ODBAXDT_data();
            public ODBAXDT_data data25 = new ODBAXDT_data();
            public ODBAXDT_data data26 = new ODBAXDT_data();
            public ODBAXDT_data data27 = new ODBAXDT_data();
            public ODBAXDT_data data28 = new ODBAXDT_data();
            public ODBAXDT_data data29 = new ODBAXDT_data();
            public ODBAXDT_data data30 = new ODBAXDT_data();
            public ODBAXDT_data data31 = new ODBAXDT_data();
            public ODBAXDT_data data32 = new ODBAXDT_data();
            public ODBAXDT_data data33 = new ODBAXDT_data();
            public ODBAXDT_data data34 = new ODBAXDT_data();
            public ODBAXDT_data data35 = new ODBAXDT_data();
            public ODBAXDT_data data36 = new ODBAXDT_data();
            public ODBAXDT_data data37 = new ODBAXDT_data();
            public ODBAXDT_data data38 = new ODBAXDT_data();
            public ODBAXDT_data data39 = new ODBAXDT_data();
            public ODBAXDT_data data40 = new ODBAXDT_data();
            public ODBAXDT_data data41 = new ODBAXDT_data();
            public ODBAXDT_data data42 = new ODBAXDT_data();
            public ODBAXDT_data data43 = new ODBAXDT_data();
            public ODBAXDT_data data44 = new ODBAXDT_data();
            public ODBAXDT_data data45 = new ODBAXDT_data();
            public ODBAXDT_data data46 = new ODBAXDT_data();
            public ODBAXDT_data data47 = new ODBAXDT_data();
            public ODBAXDT_data data48 = new ODBAXDT_data();
            public ODBAXDT_data data49 = new ODBAXDT_data();
            public ODBAXDT_data data50 = new ODBAXDT_data();
            public ODBAXDT_data data51 = new ODBAXDT_data();
            public ODBAXDT_data data52 = new ODBAXDT_data();
            public ODBAXDT_data data53 = new ODBAXDT_data();
            public ODBAXDT_data data54 = new ODBAXDT_data();
            public ODBAXDT_data data55 = new ODBAXDT_data();
            public ODBAXDT_data data56 = new ODBAXDT_data();
            public ODBAXDT_data data57 = new ODBAXDT_data();
            public ODBAXDT_data data58 = new ODBAXDT_data();
            public ODBAXDT_data data59 = new ODBAXDT_data();
            public ODBAXDT_data data60 = new ODBAXDT_data();
            public ODBAXDT_data data61 = new ODBAXDT_data();
            public ODBAXDT_data data62 = new ODBAXDT_data();
            public ODBAXDT_data data63 = new ODBAXDT_data();
            public ODBAXDT_data data64 = new ODBAXDT_data();
            public ODBAXDT_data data65 = new ODBAXDT_data();
            public ODBAXDT_data data66 = new ODBAXDT_data();
            public ODBAXDT_data data67 = new ODBAXDT_data();
            public ODBAXDT_data data68 = new ODBAXDT_data();
            public ODBAXDT_data data69 = new ODBAXDT_data();
            public ODBAXDT_data data70 = new ODBAXDT_data();
            public ODBAXDT_data data71 = new ODBAXDT_data();
            public ODBAXDT_data data72 = new ODBAXDT_data();
            public ODBAXDT_data data73 = new ODBAXDT_data();
            public ODBAXDT_data data74 = new ODBAXDT_data();
            public ODBAXDT_data data75 = new ODBAXDT_data();
            public ODBAXDT_data data76 = new ODBAXDT_data();
            public ODBAXDT_data data77 = new ODBAXDT_data();
            public ODBAXDT_data data78 = new ODBAXDT_data();
            public ODBAXDT_data data79 = new ODBAXDT_data();
            public ODBAXDT_data data80 = new ODBAXDT_data();
            public ODBAXDT_data data81 = new ODBAXDT_data();
            public ODBAXDT_data data82 = new ODBAXDT_data();
            public ODBAXDT_data data83 = new ODBAXDT_data();
            public ODBAXDT_data data84 = new ODBAXDT_data();
            public ODBAXDT_data data85 = new ODBAXDT_data();
            public ODBAXDT_data data86 = new ODBAXDT_data();
            public ODBAXDT_data data87 = new ODBAXDT_data();
            public ODBAXDT_data data88 = new ODBAXDT_data();
            public ODBAXDT_data data89 = new ODBAXDT_data();
            public ODBAXDT_data data90 = new ODBAXDT_data();
            public ODBAXDT_data data91 = new ODBAXDT_data();
            public ODBAXDT_data data92 = new ODBAXDT_data();
            public ODBAXDT_data data93 = new ODBAXDT_data();
            public ODBAXDT_data data94 = new ODBAXDT_data();
            public ODBAXDT_data data95 = new ODBAXDT_data();
            public ODBAXDT_data data96 = new ODBAXDT_data();
            public ODBAXDT_data data97 = new ODBAXDT_data();
            public ODBAXDT_data data98 = new ODBAXDT_data();
            public ODBAXDT_data data99 = new ODBAXDT_data();
            public ODBAXDT_data data100 = new ODBAXDT_data();
            public ODBAXDT_data data101 = new ODBAXDT_data();
            public ODBAXDT_data data102 = new ODBAXDT_data();
            public ODBAXDT_data data103 = new ODBAXDT_data();
            public ODBAXDT_data data104 = new ODBAXDT_data();
            public ODBAXDT_data data105 = new ODBAXDT_data();
            public ODBAXDT_data data106 = new ODBAXDT_data();
            public ODBAXDT_data data107 = new ODBAXDT_data();
            public ODBAXDT_data data108 = new ODBAXDT_data();
            public ODBAXDT_data data109 = new ODBAXDT_data();
            public ODBAXDT_data data110 = new ODBAXDT_data();
            public ODBAXDT_data data111 = new ODBAXDT_data();
            public ODBAXDT_data data112 = new ODBAXDT_data();
            public ODBAXDT_data data113 = new ODBAXDT_data();
            public ODBAXDT_data data114 = new ODBAXDT_data();
            public ODBAXDT_data data115 = new ODBAXDT_data();
            public ODBAXDT_data data116 = new ODBAXDT_data();
            public ODBAXDT_data data117 = new ODBAXDT_data();
            public ODBAXDT_data data118 = new ODBAXDT_data();
            public ODBAXDT_data data119 = new ODBAXDT_data();
            public ODBAXDT_data data120 = new ODBAXDT_data();
            public ODBAXDT_data data121 = new ODBAXDT_data();
            public ODBAXDT_data data122 = new ODBAXDT_data();
            public ODBAXDT_data data123 = new ODBAXDT_data();
            public ODBAXDT_data data124 = new ODBAXDT_data();
            public ODBAXDT_data data125 = new ODBAXDT_data();
            public ODBAXDT_data data126 = new ODBAXDT_data();
            public ODBAXDT_data data127 = new ODBAXDT_data();
            public ODBAXDT_data data128 = new ODBAXDT_data();
        }

        /* cnc_rdspcss:read constant surface speed data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBCSS
        {
            public int srpm;       /* order spindle speed */
            public int sspm;       /* order constant spindle speed */
            public int smax;       /* order maximum spindle speed */
        }

        /* cnc_rdpdf_drive:read program drive directory */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPDFDRV
        {
            public short max_num;    /* maximum drive number */
            public short dummy;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive1 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive2 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive3 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive4 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive5 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive6 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive7 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive8 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive9 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive10 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive11 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive12 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive13 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive14 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive15 = new string(' ', 12);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string drive16 = new string(' ', 12);
        }

        /* cnc_rdpdf_inf:read program drive information */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPDFINF
        {
            public int used_page;  /* used capacity */
            public int all_page;   /* all capacity */
            public int used_dir;   /* used directory number */
            public int all_dir;    /* all directory number */
        }

        /* cnc_rdpdf_subdir:read directory (sub directories) */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IDBPDFSDIR
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 212)]
            public string path = new string(' ', 212);  /* path name */
            public short req_num;    /* entry number */
            public short dummy;
        }

        /* cnc_rdpdf_subdir:read directory (sub directories) */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPDFSDIR
        {
            public short sub_exist;    /* existence of sub directory */
            public short dummy;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string d_f = new string(' ', 36);  /* path name */
        }

        /* cnc_rdpdf_alldir:read directory (all files) */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IDBPDFADIR
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 212)]
            public string path = new string(' ', 212);  /* path name */
            public short req_num;    /* entry number */
            public short size_kind;  /* kind of size */
            public short type;       /* kind of format */
            public short dummy;
        }

        /* cnc_rdpdf_alldir:read directory (all files) */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPDFADIR
        {
            public short data_kind;  /* kinf of data */
            public short year;       /* last date and time */
            public short mon;        /* last date and time */
            public short day;        /* last date and time */
            public short hour;       /* last date and time */
            public short min;        /* last date and time */
            public short sec;        /* last date and time */
            public short dummy;
            public int dummy2;
            public int size;       /* size */
            public int attr;       /* attribute */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string d_f = new string(' ', 36);      /* path name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 52)]
            public string comment = new string(' ', 52);  /* comment */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string o_time = new string(' ', 12);   /* machining time stamp */
        }

        /* cnc_rdpdf_subdirn:read file count the directory has */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPDFNFIL
        {
            public short dir_num;    /* directory */
            public short file_num;   /* file */
        }

        /* cnc_wrpdf_attr:change attribute of program file and directory */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBPDFTDIR
        {
            public uint slct;       /* selection */
            public uint attr;       /* data */
        }

        /*---------------------------*/
        /* CNC: NC file data related */
        /*---------------------------*/

        /* cnc_rdtofs:read tool offset value */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTOFS
        {
            public short datano; /* data number */
            public short type;   /* data type */
            public int data;   /* data */
        }

        /* cnc_rdtofsr:read tool offset value(area specified) */
        /* cnc_wrtofsr:write tool offset value(area specified) */
        [StructLayout(LayoutKind.Explicit)]
        public class OFS_1
        {
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] m_ofs = new int[5];     /* M Each */
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] m_ofs_a = new int[5];   /* M-A All */
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] t_tip = new short[5];   /* T Each, 2-byte */
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] t_ofs = new int[5];     /* T Each, 4-byte */
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OFS_2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * 5)]
            public int[] m_ofs_b = new int[10];  /* M-B All */
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OFS_3
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 5)]
            public int[] m_ofs_c = new int[20];   /* M-C All */
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class T_OFS_A
        {
            public short tip;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] data;
        } /* T-A All */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class T_OFS_A_data
        {
            public T_OFS_A data1 = new T_OFS_A();
            public T_OFS_A data2 = new T_OFS_A();
            public T_OFS_A data3 = new T_OFS_A();
            public T_OFS_A data4 = new T_OFS_A();
            public T_OFS_A data5 = new T_OFS_A();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class T_OFS_B
        {
            public short tip;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] data;
        } /* T-B All */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class T_OFS_B_data
        {
            public T_OFS_B data1 = new T_OFS_B();
            public T_OFS_B data2 = new T_OFS_B();
            public T_OFS_B data3 = new T_OFS_B();
            public T_OFS_B data4 = new T_OFS_B();
            public T_OFS_B data5 = new T_OFS_B();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTO_1_1
        {
            public short datano_s;  /* start offset number */
            public short type;      /* offset type */
            public short datano_e;  /* end offset number */
            public OFS_1 ofs = new OFS_1();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTO_1_2
        {
            public short datano_s;  /* start offset number */
            public short type;      /* offset type */
            public short datano_e;  /* end offset number */
            public OFS_2 ofs = new OFS_2();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTO_1_3
        {
            public short datano_s;  /* start offset number */
            public short type;      /* offset type */
            public short datano_e;  /* end offset number */
            public OFS_3 ofs = new OFS_3();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTO_2
        {
            public short datano_s;  /* start offset number */
            public short type;      /* offset type */
            public short datano_e;  /* end offset number */
            public T_OFS_A_data tofsa = new T_OFS_A_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTO_3
        {
            public short datano_s;  /* start offset number */
            public short type;      /* offset type */
            public short datano_e;  /* end offset number */
            public T_OFS_B_data tofsb = new T_OFS_B_data();
        }

        /* cnc_rdzofs:read work zero offset value */
        /* cnc_wrzofs:write work zero offset value */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBZOFS
        {
            public short datano;    /* offset NO. */
            public short type;      /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] data = new int[MAX_AXIS];       /* data value */
        }

        /* cnc_rdzofsr:read work zero offset value(area specified) */
        /* cnc_wrzofsr:write work zero offset value(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBZOR
        {
            public short datano_s;  /* start offset number */
            public short type;      /* axis number */
            public short datano_e;  /* end offset number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7 * MAX_AXIS)]
            public int[] data = new int[7 * MAX_AXIS];      /* offset value */
        } /* In case that the number of axes is MAX_AXIS, the number of data is 7 */

        /* cnc_rdmsptype:read mesured point value */
        /* cnc_wrmsptype:write mesured point value */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMSTP
        {
            public short datano_s;  /* start offset number */
            public short dummy;     /* dummy */
            public short datano_e;  /* end offset number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public sbyte[] data = new sbyte[7];      /* mesured point value */
        }

        /* cnc_rdparam:read parameter */
        /* cnc_wrparam:write parameter */
        /* cnc_rdset:read setting data */
        /* cnc_wrset:write setting data */
        /* cnc_rdparar:read parameter(area specified) */
        /* cnc_wrparas:write parameter(plural specified) */
        /* cnc_rdsetr:read setting data(area specified) */
        /* cnc_wrsets:write setting data(plural specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REALPRM
        {
            public int prm_val;     /* data of real parameter */
            public int dec_val;     /* decimal point of real parameter */
        }
#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class REALPRMS
    {
        public REALPRM rdata1=new REALPRM();
        public REALPRM rdata2=new REALPRM();
        public REALPRM rdata3=new REALPRM();
        public REALPRM rdata4=new REALPRM();
        public REALPRM rdata5=new REALPRM();
        public REALPRM rdata6=new REALPRM();
        public REALPRM rdata7=new REALPRM();
        public REALPRM rdata8=new REALPRM();
        public REALPRM rdata9=new REALPRM();
        public REALPRM rdata10=new REALPRM();
        public REALPRM rdata11=new REALPRM();
        public REALPRM rdata12=new REALPRM();
        public REALPRM rdata13=new REALPRM();
        public REALPRM rdata14=new REALPRM();
        public REALPRM rdata15=new REALPRM();
        public REALPRM rdata16=new REALPRM();
        public REALPRM rdata17=new REALPRM();
        public REALPRM rdata18=new REALPRM();
        public REALPRM rdata19=new REALPRM();
        public REALPRM rdata20=new REALPRM();
        public REALPRM rdata21=new REALPRM();
        public REALPRM rdata22=new REALPRM();
        public REALPRM rdata23=new REALPRM();
        public REALPRM rdata24=new REALPRM();
    } /* In case that the number of alarm is 24 */
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class REALPRMS
    {
        public REALPRM rdata1=new REALPRM();
        public REALPRM rdata2=new REALPRM();
        public REALPRM rdata3=new REALPRM();
        public REALPRM rdata4=new REALPRM();
        public REALPRM rdata5=new REALPRM();
        public REALPRM rdata6=new REALPRM();
        public REALPRM rdata7=new REALPRM();
        public REALPRM rdata8=new REALPRM();
        public REALPRM rdata9=new REALPRM();
        public REALPRM rdata10=new REALPRM();
    } /* In case that the number of alarm is 10 */
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REALPRMS
        {
            public REALPRM rdata1 = new REALPRM();
            public REALPRM rdata2 = new REALPRM();
            public REALPRM rdata3 = new REALPRM();
            public REALPRM rdata4 = new REALPRM();
            public REALPRM rdata5 = new REALPRM();
            public REALPRM rdata6 = new REALPRM();
            public REALPRM rdata7 = new REALPRM();
            public REALPRM rdata8 = new REALPRM();
        } /* In case that the number of alarm is 8 */
#endif

        [StructLayout(LayoutKind.Explicit)]
        public class IODBPSD_1
        {
            [FieldOffset(0)]
            public short datano;    /* data number */
            [FieldOffset(2)]
            public short type;      /* axis number */
            [FieldOffset(4)]
            public byte cdata;     /* parameter / setting data */
            [FieldOffset(4)]
            public short idata;
            [FieldOffset(4)]
            public int ldata;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_2
        {
            public short datano;    /* data number */
            public short type;      /* axis number */
            public REALPRM rdata = new REALPRM();
        }
        [StructLayout(LayoutKind.Explicit)]
        public class IODBPSD_3
        {
            [FieldOffset(0)]
            public short datano;    /* data number */
            [FieldOffset(2)]
            public short type;      /* axis number */
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas = new byte[MAX_AXIS];
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas = new short[MAX_AXIS];
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas = new int[MAX_AXIS];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_4
        {
            public short datano;    /* data number */
            public short type;      /* axis number */
            public REALPRMS rdatas = new REALPRMS();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_A
        {
            public IODBPSD_1 data1 = new IODBPSD_1();
            public IODBPSD_1 data2 = new IODBPSD_1();
            public IODBPSD_1 data3 = new IODBPSD_1();
            public IODBPSD_1 data4 = new IODBPSD_1();
            public IODBPSD_1 data5 = new IODBPSD_1();
            public IODBPSD_1 data6 = new IODBPSD_1();
            public IODBPSD_1 data7 = new IODBPSD_1();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_B
        {
            public IODBPSD_2 data1 = new IODBPSD_2();
            public IODBPSD_2 data2 = new IODBPSD_2();
            public IODBPSD_2 data3 = new IODBPSD_2();
            public IODBPSD_2 data4 = new IODBPSD_2();
            public IODBPSD_2 data5 = new IODBPSD_2();
            public IODBPSD_2 data6 = new IODBPSD_2();
            public IODBPSD_2 data7 = new IODBPSD_2();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_C
        {
            public IODBPSD_3 data1 = new IODBPSD_3();
            public IODBPSD_3 data2 = new IODBPSD_3();
            public IODBPSD_3 data3 = new IODBPSD_3();
            public IODBPSD_3 data4 = new IODBPSD_3();
            public IODBPSD_3 data5 = new IODBPSD_3();
            public IODBPSD_3 data6 = new IODBPSD_3();
            public IODBPSD_3 data7 = new IODBPSD_3();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSD_D
        {
            public IODBPSD_4 data1 = new IODBPSD_4();
            public IODBPSD_4 data2 = new IODBPSD_4();
            public IODBPSD_4 data3 = new IODBPSD_4();
            public IODBPSD_4 data4 = new IODBPSD_4();
            public IODBPSD_4 data5 = new IODBPSD_4();
            public IODBPSD_4 data6 = new IODBPSD_4();
            public IODBPSD_4 data7 = new IODBPSD_4();
        } /* (sample) must be modified */

        /* cnc_rdparam_ext:read parameters */
        /* cnc_rddiag_ext:read diagnosis data */
        /* cnc_start_async_wrparam:async parameter write start */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRMNO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] prm = new int[10];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRM_data
        {
            public int prm_val;   /* parameter / setting data */
            public int dec_val;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRM1
        {
            public IODBPRM_data data1 = new IODBPRM_data();
            public IODBPRM_data data2 = new IODBPRM_data();
            public IODBPRM_data data3 = new IODBPRM_data();
            public IODBPRM_data data4 = new IODBPRM_data();
            public IODBPRM_data data5 = new IODBPRM_data();
            public IODBPRM_data data6 = new IODBPRM_data();
            public IODBPRM_data data7 = new IODBPRM_data();
            public IODBPRM_data data8 = new IODBPRM_data();
            public IODBPRM_data data9 = new IODBPRM_data();
            public IODBPRM_data data10 = new IODBPRM_data();
            public IODBPRM_data data11 = new IODBPRM_data();
            public IODBPRM_data data12 = new IODBPRM_data();
            public IODBPRM_data data13 = new IODBPRM_data();
            public IODBPRM_data data14 = new IODBPRM_data();
            public IODBPRM_data data15 = new IODBPRM_data();
            public IODBPRM_data data16 = new IODBPRM_data();
            public IODBPRM_data data17 = new IODBPRM_data();
            public IODBPRM_data data18 = new IODBPRM_data();
            public IODBPRM_data data19 = new IODBPRM_data();
            public IODBPRM_data data20 = new IODBPRM_data();
            public IODBPRM_data data21 = new IODBPRM_data();
            public IODBPRM_data data22 = new IODBPRM_data();
            public IODBPRM_data data23 = new IODBPRM_data();
            public IODBPRM_data data24 = new IODBPRM_data();
            public IODBPRM_data data25 = new IODBPRM_data();
            public IODBPRM_data data26 = new IODBPRM_data();
            public IODBPRM_data data27 = new IODBPRM_data();
            public IODBPRM_data data28 = new IODBPRM_data();
            public IODBPRM_data data29 = new IODBPRM_data();
            public IODBPRM_data data30 = new IODBPRM_data();
            public IODBPRM_data data31 = new IODBPRM_data();
            public IODBPRM_data data32 = new IODBPRM_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRM2
        {
            public int datano;    /* data number */
            public short type;      /* data type */
            public short axis;      /* axis information */
            public short info;      /* misc information */
            public short unit;      /* unit information */
            public IODBPRM1 data = new IODBPRM1();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRM
        {
            public IODBPRM2 prm1 = new IODBPRM2();
            public IODBPRM2 prm2 = new IODBPRM2();
            public IODBPRM2 prm3 = new IODBPRM2();
            public IODBPRM2 prm4 = new IODBPRM2();
            public IODBPRM2 prm5 = new IODBPRM2();
            public IODBPRM2 prm6 = new IODBPRM2();
            public IODBPRM2 prm7 = new IODBPRM2();
            public IODBPRM2 prm8 = new IODBPRM2();
            public IODBPRM2 prm9 = new IODBPRM2();
            public IODBPRM2 prm10 = new IODBPRM2();
        } /* In case that the number of alarm is 10 */

        /* cnc_rdpitchr:read pitch error compensation data(area specified) */
        /* cnc_wrpitchr:write pitch error compensation data(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPI
        {
            public short datano_s;  /* start pitch number */
            public short dummy;     /* dummy */
            public short datano_e;  /* end pitch number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public sbyte[] data = new sbyte[5];    /* offset value */
        } /* In case that the number of data is 5 */

        /* cnc_rdmacro:read custom macro variable */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBM
        {
            public short datano;    /* variable number */
            public short dummy;     /* dummy */
            public int mcr_val;   /* macro variable */
            public short dec_val;   /* decimal point */
        }

        /* cnc_rdmacror:read custom macro variables(area specified) */
        /* cnc_wrmacror:write custom macro variables(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMR_data
        {
            public int mcr_val;   /* macro variable */
            public short dec_val;   /* decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMR1
        {
            public IODBMR_data data1 = new IODBMR_data();
            public IODBMR_data data2 = new IODBMR_data();
            public IODBMR_data data3 = new IODBMR_data();
            public IODBMR_data data4 = new IODBMR_data();
            public IODBMR_data data5 = new IODBMR_data();
        }  /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMR
        {
            public short datano_s;  /* start macro number */
            public short dummy;     /* dummy */
            public short datano_e;  /* end macro number */
            public IODBMR1 data = new IODBMR1();
        }

        /* cnc_rdpmacro:read P code macro variable */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPM
        {
            public int datano;    /* variable number */
            public short dummy;     /* dummy */
            public int mcr_val;   /* macro variable */
            public short dec_val;   /* decimal point */
        }

        /* cnc_rdpmacror:read P code macro variables(area specified) */
        /* cnc_wrpmacror:write P code macro variables(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPR_data
        {
            public int mcr_val;   /* macro variable */
            public short dec_val;   /* decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPR1
        {
            public IODBPR_data data1 = new IODBPR_data();
            public IODBPR_data data2 = new IODBPR_data();
            public IODBPR_data data3 = new IODBPR_data();
            public IODBPR_data data4 = new IODBPR_data();
            public IODBPR_data data5 = new IODBPR_data();
        }  /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPR
        {
            public int datano_s;  /* start macro number */
            public short dummy;     /* dummy */
            public int datano_e;  /* end macro number */
            public IODBPR1 data = new IODBPR1();
        }

        /* cnc_rdtofsinfo:read tool offset information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLINF
        {
            public short ofs_type;
            public short use_no;
        }

        /* cnc_rdtofsinfo2:read tool offset information(2) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLINF2
        {
            public short ofs_type;
            public short use_no;
            public short ofs_enable;
        }

        /* cnc_rdmacroinfo:read custom macro variable information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMVINF
        {
            public short use_no1;
            public short use_no2;
        }

        /* cnc_rdpmacroinfo:read P code macro variable information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMINF
        {
            public short use_no1;
#if PCD_UWORD
        public ushort  use_no2;
#else
            public short use_no2;
#endif
            public short v2_type;
        }

        /* cnc_tofs_rnge:read validity of tool offset */
        /* cnc_zofs_rnge:read validity of work zero offset */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDATRNG
        {
            public int data_min;   /* lower limit */
            public int data_max;   /* upper limit */
            public int status;     /* status of setting */
        }

        /* cnc_rdhsprminfo:read the information for function cnc_rdhsparam() */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class HSPINFO_data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data1 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data2 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data3 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data4 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data5 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data6 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data7 = new byte[16];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data8 = new byte[16];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class HSPINFO
        {
            public HSPINFO_data prminfo1 = new HSPINFO_data();
            public HSPINFO_data prminfo2 = new HSPINFO_data();
            public HSPINFO_data prminfo3 = new HSPINFO_data();
            public HSPINFO_data prminfo4 = new HSPINFO_data();
            public HSPINFO_data prminfo5 = new HSPINFO_data();
            public HSPINFO_data prminfo6 = new HSPINFO_data();
            public HSPINFO_data prminfo7 = new HSPINFO_data();
            public HSPINFO_data prminfo8 = new HSPINFO_data();
            public HSPINFO_data prminfo9 = new HSPINFO_data();
            public HSPINFO_data prminfo10 = new HSPINFO_data();
        }

        /* cnc_rdhsparam:read parameters at the high speed */
        [StructLayout(LayoutKind.Explicit)]
        public class HSPDATA_1
        {
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas1 = new byte[MAX_AXIS];
            [FieldOffset(4 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas2 = new byte[MAX_AXIS];
            [FieldOffset(4 * 2 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas3 = new byte[MAX_AXIS];
            [FieldOffset(4 * 3 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas4 = new byte[MAX_AXIS];
            [FieldOffset(4 * 4 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas5 = new byte[MAX_AXIS];
            [FieldOffset(4 * 5 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas6 = new byte[MAX_AXIS];
            [FieldOffset(4 * 6 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas7 = new byte[MAX_AXIS];
            [FieldOffset(4 * 7 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas8 = new byte[MAX_AXIS];
            [FieldOffset(4 * 8 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas9 = new byte[MAX_AXIS];
            [FieldOffset(4 * 9 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas10 = new byte[MAX_AXIS];
        }

        [StructLayout(LayoutKind.Explicit)]
        public class HSPDATA_2
        {
            [FieldOffset(0),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas1 = new short[MAX_AXIS];
            [FieldOffset(2 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas2 = new short[MAX_AXIS];
            [FieldOffset(2 * 2 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas3 = new short[MAX_AXIS];
            [FieldOffset(2 * 3 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas4 = new short[MAX_AXIS];
            [FieldOffset(2 * 4 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas5 = new short[MAX_AXIS];
            [FieldOffset(2 * 5 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas6 = new short[MAX_AXIS];
            [FieldOffset(2 * 6 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas7 = new short[MAX_AXIS];
            [FieldOffset(2 * 7 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas8 = new short[MAX_AXIS];
            [FieldOffset(2 * 8 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas9 = new short[MAX_AXIS];
            [FieldOffset(2 * 9 * MAX_AXIS),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas10 = new short[MAX_AXIS];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class HSPDATA_3
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas1 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas2 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas3 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas4 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas5 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas6 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas7 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas8 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas9 = new int[MAX_AXIS];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas10 = new int[MAX_AXIS];
        }

        /*----------------------------------------*/
        /* CNC: Tool life management data related */
        /*----------------------------------------*/

        /* cnc_rdgrpid:read tool life management data(tool group number) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLIFE1
        {
            public short dummy; /* dummy */
            public short type;  /* data type */
            public int data;  /* data */
        }

        /* cnc_rdngrp:read tool life management data(number of tool groups) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLIFE2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;  /* dummy */
            public int data;   /* data */
        }

        /* cnc_rdntool:read tool life management data(number of tools) */
        /* cnc_rdlife:read tool life management data(tool life) */
        /* cnc_rdcount:read tool life management data(tool lift counter) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLIFE3
        {
            public short datano;    /* data number */
            public short dummy;     /* dummy */
            public int data;      /* data */
        }

        /* cnc_rd1length:read tool life management data(tool length number-1) */
        /* cnc_rd2length:read tool life management data(tool length number-2) */
        /* cnc_rd1radius:read tool life management data(cutter compensation no.-1) */
        /* cnc_rd2radius:read tool life management data(cutter compensation no.-2) */
        /* cnc_t1info:read tool life management data(tool information-1) */
        /* cnc_t2info:read tool life management data(tool information-2) */
        /* cnc_toolnum:read tool life management data(tool number) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLIFE4
        {
            public short datano;    /* data number */
            public short type;      /* data type */
            public int data;      /* data */
        }

        /* cnc_rdgrpid2:read tool life management data(tool group number) 2 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLIFE5
        {
            public int dummy; /* dummy */
            public int type;  /* data type */
            public int data;  /* data */
        }

        /* cnc_rdtoolrng:read tool life management data(tool number, tool life, tool life counter)(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTR_data
        {
            public int ntool;     /* tool number */
            public int life;      /* tool life */
            public int count;     /* tool life counter */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTR1
        {
            public IODBTR_data data1 = new IODBTR_data();
            public IODBTR_data data2 = new IODBTR_data();
            public IODBTR_data data3 = new IODBTR_data();
            public IODBTR_data data4 = new IODBTR_data();
            public IODBTR_data data5 = new IODBTR_data();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTR
        {
            public short datano_s;  /* start group number */
            public short dummy;     /* dummy */
            public short datano_e;  /* end group number */
            public IODBTR1 data = new IODBTR1();
        }

        /* cnc_rdtoolgrp:read tool life management data(all data within group) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTG_data
        {
            public int tuse_num;      /* tool number */
            public int tool_num;      /* tool life */
            public int length_num;    /* tool life counter */
            public int radius_num;    /* tool life counter */
            public int tinfo;         /* tool life counter */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTG1
        {
            public ODBTG_data data1 = new ODBTG_data();
            public ODBTG_data data2 = new ODBTG_data();
            public ODBTG_data data3 = new ODBTG_data();
            public ODBTG_data data4 = new ODBTG_data();
            public ODBTG_data data5 = new ODBTG_data();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTG
        {
            public short grp_num;   /* start group number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy;  /* dummy */
            public int ntool;     /* tool number */
            public int life;      /* tool life */
            public int count;     /* tool life counter */
            public ODBTG1 data = new ODBTG1();
        }

        /* cnc_wrcountr:write tool life management data(tool life counter) (area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBWRC_data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] dummy;     /* dummy */
            public int count;     /* tool life counter */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBWRC1
        {
            public IDBWRC_data data1 = new IDBWRC_data();
            public IDBWRC_data data2 = new IDBWRC_data();
            public IDBWRC_data data3 = new IDBWRC_data();
            public IDBWRC_data data4 = new IDBWRC_data();
            public IDBWRC_data data5 = new IDBWRC_data();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBWRC
        {
            public short datano_s;  /* start group number */
            public short dummy;     /* dummy */
            public short datano_e;  /* end group number */
            public IDBWRC1 data = new IDBWRC1();
        }

        /* cnc_rdusegrpid:read tool life management data(used tool group number) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBUSEGR
        {
            public short datano; /* dummy */
            public short type;   /* dummy */
            public int next;   /* next use group number */
            public int use;    /* using group number */
            public int slct;   /* selecting group number */
        }

        /* cnc_rdmaxgrp:read tool life management data(max. number of tool groups) */
        /* cnc_rdmaxtool:read tool life management data(maximum number of tool within group) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBLFNO
        {
            public short datano; /* dummy */
            public short type;   /* dummy */
            public short data;   /* number of data */
        }

        /* cnc_rdusetlno:read tool life management data(used tool no within group) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLUSE
        {
            public short s_grp;  /* start group number */
            public short dummy;  /* dummy */
            public short e_grp;  /* end group number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] data;   /* tool using number */
        } /* In case that the number of group is 5 */

        /* cnc_rd1tlifedata:read tool life management data(tool data1) */
        /* cnc_rd2tlifedata:read tool life management data(tool data2) */
        /* cnc_wr1tlifedata:write tool life management data(tool data1) */
        /* cnc_wr2tlifedata:write tool life management data(tool data2) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTD
        {
            public short datano;     /* tool group number */
            public short type;       /* tool using number */
            public int tool_num;   /* tool number */
            public int h_code;     /* H code */
            public int d_code;     /* D code */
            public int tool_inf;   /* tool information */
        }

        /* cnc_rd1tlifedat2:read tool life management data(tool data1) 2 */
        /* cnc_wr1tlifedat2:write tool life management data(tool data1) 2 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTD2
        {
            public short datano;     /* tool group number */
            public short dummy;      /* dummy */
            public int type;       /* tool using number */
            public int tool_num;   /* tool number */
            public int h_code;     /* H code */
            public int d_code;     /* D code */
            public int tool_inf;   /* tool information */
        }

        /* cnc_rdgrpinfo:read tool life management data(tool group information) */
        /* cnc_wrgrpinfo:write tool life management data(tool group information) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI_data
        {
            public int n_tool;         /* number of tool */
            public int count_value;    /* tool life */
            public int counter;        /* tool life counter */
            public int count_type;     /* tool life counter type */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI1
        {
            public IODBTGI_data data1 = new IODBTGI_data();
            public IODBTGI_data data2 = new IODBTGI_data();
            public IODBTGI_data data3 = new IODBTGI_data();
            public IODBTGI_data data4 = new IODBTGI_data();
            public IODBTGI_data data5 = new IODBTGI_data();
        } /* In case that the number of data is 5 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI
        {
            public short s_grp;  /* start group number */
            public short dummy;  /* dummy */
            public short e_grp;  /* end group number */
            public IODBTGI1 data = new IODBTGI1();
        }

        /* cnc_rdgrpinfo2:read tool life management data(tool group information 2) */
        /* cnc_wrgrpinfo2:write tool life management data(tool group information 2) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI2
        {
            public short s_grp;      /* start group number */
            public short dummy;      /* dummy */
            public short e_grp;      /* end group number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] opt_grpno = new int[5];  /* optional group number of tool */
        } /* In case that the number of group is 5 */

        /* cnc_rdgrpinfo3:read tool life management data(tool group information 3) */
        /* cnc_wrgrpinfo3:write tool life management data(tool group information 3) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI3
        {
            public short s_grp;      /* start group number */
            public short dummy;      /* dummy */
            public short e_grp;      /* end group number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] life_rest = new int[5];  /* tool life rest count */
        } /* In case that the number of group is 5 */

        /* cnc_rdgrpinfo4:read tool life management data(tool group information 4) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTGI4
        {
            public short grp_no;
            public int n_tool;
            public int count_value;
            public int counter;
            public int count_type;
            public int opt_grpno;
            public int life_rest;
        }

        /* cnc_instlifedt:insert tool life management data(tool data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBITD
        {
            public short datano; /* tool group number */
            public short type;   /* tool using number */
            public int data;   /* tool number */
        }

        /* cnc_rdtlinfo:read tool life management data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBTLINFO
        {
            public int max_group;  /* maximum number of tool groups */
            public int max_tool;   /* maximum number of tool within group */
            public int max_minute; /* maximum number of life count (minutes) */
            public int max_cycle;  /* maximum number of life count (cycles) */
        }

        /* cnc_rdtlusegrp:read tool life management data(used tool group number) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBUSEGRP
        {
            public int next;       /* next use group number */
            public int use;        /* using group number */
            public int slct;       /* selecting group number */
            public int opt_next;   /* next use optional group number */
            public int opt_use;    /* using optional group number */
            public int opt_slct;   /* selecting optional group number */
        }

        /* cnc_rdtlgrp:read tool life management data(tool group information 2) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLGRP_data
        {
            public int ntool;      /* number of all tool */
            public int nfree;      /* number of free tool */
            public int life;       /* tool life */
            public int count;      /* tool life counter */
            public int use_tool;   /* using tool number */
            public int opt_grpno;  /* optional group number */
            public int life_rest;  /* tool life rest count */
            public short rest_sig;   /* tool life rest signal */
            public short count_type; /* tool life counter type */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLGRP
        {
            public IODBTLGRP_data data1 = new IODBTLGRP_data();
            public IODBTLGRP_data data2 = new IODBTLGRP_data();
            public IODBTLGRP_data data3 = new IODBTLGRP_data();
            public IODBTLGRP_data data4 = new IODBTLGRP_data();
            public IODBTLGRP_data data5 = new IODBTLGRP_data();
        } /* In case that the number of group is 5 */

        /* cnc_rdtltool:read tool life management data (tool data1) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLTOOL_data
        {
            public int tool_num;   /* tool number */
            public int h_code;     /* H code */
            public int d_code;     /* D code */
            public int tool_inf;   /* tool information */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLTOOL
        {
            public IODBTLTOOL_data data1 = new IODBTLTOOL_data();
            public IODBTLTOOL_data data2 = new IODBTLTOOL_data();
            public IODBTLTOOL_data data3 = new IODBTLTOOL_data();
            public IODBTLTOOL_data data4 = new IODBTLTOOL_data();
            public IODBTLTOOL_data data5 = new IODBTLTOOL_data();
        } /* In case that the number of group is 5 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXGP_data
        {
            public int grp_no;     /* group number */
            public int opt_grpno;  /* optional group number */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXGP
        {
            public ODBEXGP_data data1 = new ODBEXGP_data();
            public ODBEXGP_data data2 = new ODBEXGP_data();
            public ODBEXGP_data data3 = new ODBEXGP_data();
            public ODBEXGP_data data4 = new ODBEXGP_data();
            public ODBEXGP_data data5 = new ODBEXGP_data();
            public ODBEXGP_data data6 = new ODBEXGP_data();
            public ODBEXGP_data data7 = new ODBEXGP_data();
            public ODBEXGP_data data8 = new ODBEXGP_data();
            public ODBEXGP_data data9 = new ODBEXGP_data();
            public ODBEXGP_data data10 = new ODBEXGP_data();
            public ODBEXGP_data data11 = new ODBEXGP_data();
            public ODBEXGP_data data12 = new ODBEXGP_data();
            public ODBEXGP_data data13 = new ODBEXGP_data();
            public ODBEXGP_data data14 = new ODBEXGP_data();
            public ODBEXGP_data data15 = new ODBEXGP_data();
            public ODBEXGP_data data16 = new ODBEXGP_data();
            public ODBEXGP_data data17 = new ODBEXGP_data();
            public ODBEXGP_data data18 = new ODBEXGP_data();
            public ODBEXGP_data data19 = new ODBEXGP_data();
            public ODBEXGP_data data20 = new ODBEXGP_data();
            public ODBEXGP_data data21 = new ODBEXGP_data();
            public ODBEXGP_data data22 = new ODBEXGP_data();
            public ODBEXGP_data data23 = new ODBEXGP_data();
            public ODBEXGP_data data24 = new ODBEXGP_data();
            public ODBEXGP_data data25 = new ODBEXGP_data();
            public ODBEXGP_data data26 = new ODBEXGP_data();
            public ODBEXGP_data data27 = new ODBEXGP_data();
            public ODBEXGP_data data28 = new ODBEXGP_data();
            public ODBEXGP_data data29 = new ODBEXGP_data();
            public ODBEXGP_data data30 = new ODBEXGP_data();
            public ODBEXGP_data data31 = new ODBEXGP_data();
            public ODBEXGP_data data32 = new ODBEXGP_data();
        }

        /*-----------------------------------*/
        /* CNC: Tool management data related */
        /*-----------------------------------*/

        /* cnc_regtool:new registration of tool management data */
        /* cnc_rdtool:lead of tool management data */
        /* cnc_wrtool:write of tool management data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMNG_data
        {
            public int T_code;
            public int life_count;
            public int max_life;
            public int rest_life;
            public byte life_stat;
            public byte cust_bits;
            public ushort tool_info;
            public short H_code;
            public short D_code;
            public int spindle_speed;
            public int feedrate;
            public short magazine;
            public short pot;
            public short gno;
            public short m_ofs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] reserved = new int[4];
            public int custom1;
            public int custom2;
            public int custom3;
            public int custom4;
            public int custom5;
            public int custom6;
            public int custom7;
            public int custom8;
            public int custom9;
            public int custom10;
            public int custom11;
            public int custom12;
            public int custom13;
            public int custom14;
            public int custom15;
            public int custom16;
            public int custom17;
            public int custom18;
            public int custom19;
            public int custom20;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMNG
        {
            public IODBTLMNG_data data1 = new IODBTLMNG_data();
            public IODBTLMNG_data data2 = new IODBTLMNG_data();
            public IODBTLMNG_data data3 = new IODBTLMNG_data();
            public IODBTLMNG_data data4 = new IODBTLMNG_data();
            public IODBTLMNG_data data5 = new IODBTLMNG_data();
        } /* In case that the number of group is 5 */


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMNG_F2_data
        {
            public int T_code;
            public int life_count;
            public int max_life;
            public int rest_life;
            public byte life_stat;
            public byte cust_bits;
            public ushort tool_info;
            public short H_code;
            public short D_code;
            public int spindle_speed;
            public int feedrate;
            public short magazine;
            public short pot;
            public short G_code;
            public short W_code;
            public short gno;
            public short m_ofs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] reserved;
            public int custom1;
            public int custom2;
            public int custom3;
            public int custom4;
            public int custom5;
            public int custom6;
            public int custom7;
            public int custom8;
            public int custom9;
            public int custom10;
            public int custom11;
            public int custom12;
            public int custom13;
            public int custom14;
            public int custom15;
            public int custom16;
            public int custom17;
            public int custom18;
            public int custom19;
            public int custom20;
            public int custom21;
            public int custom22;
            public int custom23;
            public int custom24;
            public int custom25;
            public int custom26;
            public int custom27;
            public int custom28;
            public int custom29;
            public int custom30;
            public int custom31;
            public int custom32;
            public int custom33;
            public int custom34;
            public int custom35;
            public int custom36;
            public int custom37;
            public int custom38;
            public int custom39;
            public int custom40;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMNG_F2
        {
            public IODBTLMNG_F2_data data1 = new IODBTLMNG_F2_data();
            public IODBTLMNG_F2_data data2 = new IODBTLMNG_F2_data();
            public IODBTLMNG_F2_data data3 = new IODBTLMNG_F2_data();
            public IODBTLMNG_F2_data data4 = new IODBTLMNG_F2_data();
            public IODBTLMNG_F2_data data5 = new IODBTLMNG_F2_data();
        } /* In case that the number of group is 5 */

        /* cnc_wrtool2:write of individual data of tool management data */
        [StructLayout(LayoutKind.Explicit)]
        public class IDBTLM_item
        {
            [FieldOffset(0)]
            public sbyte data1;
            [FieldOffset(0)]
            public short data2;
            [FieldOffset(0)]
            public int data4;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBTLM
        {
            public short data_id;
            public IDBTLM_item item = new IDBTLM_item();
        }

        /* cnc_regmagazine:new registration of magazine management data */
        /* cnc_rdmagazine:lead of magazine management data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMAG_data
        {
            public short magazine;
            public short pot;
            public short tool_index;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMAG
        {
            public IODBTLMAG_data data1 = new IODBTLMAG_data();
            public IODBTLMAG_data data2 = new IODBTLMAG_data();
            public IODBTLMAG_data data3 = new IODBTLMAG_data();
            public IODBTLMAG_data data4 = new IODBTLMAG_data();
            public IODBTLMAG_data data5 = new IODBTLMAG_data();
        } /* In case that the number of group is 5 */

        /* cnc_delmagazine:deletion of magazine management data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMAG2_data
        {
            public short magazine;
            public short pot;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLMAG2
        {
            public IODBTLMAG2_data data1 = new IODBTLMAG2_data();
            public IODBTLMAG2_data data2 = new IODBTLMAG2_data();
            public IODBTLMAG2_data data3 = new IODBTLMAG2_data();
            public IODBTLMAG2_data data4 = new IODBTLMAG2_data();
            public IODBTLMAG2_data data5 = new IODBTLMAG2_data();
        } /* In case that the number of group is 5 */


        /*-------------------------------------*/
        /* CNC: Operation history data related */
        /*-------------------------------------*/

        /* cnc_rdophistry:read operation history data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_ALM
        {
            public short rec_type;   /* record type */
            public short alm_grp;    /* alarm group */
            public short alm_no;     /* alarm number */
            public sbyte axis_no;    /* axis number */
            public sbyte dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MDI
        {
            public short rec_type;   /* record type */
            public byte key_code;   /* key code */
            public byte pw_flag;    /* power on flag */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public sbyte[] dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_SGN
        {
            public short rec_type;   /* record type */
            public sbyte sig_name;   /* signal name */
            public byte sig_old;    /* old signal bit pattern */
            public byte sig_new;    /* new signal bit pattern */
            public sbyte dummy;
            public short sig_no;     /* signal number */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_DATE
        {
            public short rec_type;   /* record type */
            public sbyte year;       /* year */
            public sbyte month;      /* month */
            public sbyte day;        /* day */
            public sbyte pw_flag;    /* power on flag */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public sbyte[] dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_TIME
        {
            public short rec_type;   /* record flag */
            public sbyte hour;       /* hour */
            public sbyte minute;     /* minute */
            public sbyte second;     /* second */
            public sbyte pw_flag;    /* power on flag */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public sbyte[] dummy;
        }
        [StructLayout(LayoutKind.Explicit, Size = 8)]
        public class ODBHIS_data
        {
            // record type
            [FieldOffset(0)]
            public short rec_type;   /* record type */

            // alarm record
            [FieldOffset(0)]
            public short alm_rec_type;   /* record type */
            [FieldOffset(2)]
            public short alm_alm_grp;    /* alarm group */
            [FieldOffset(4)]
            public short alm_alm_no;     /* alarm number */
            [FieldOffset(6)]
            public sbyte alm_axis_no;    /* axis number */
            [FieldOffset(7)]
            public sbyte alm_dummy;

            // mdi record
            [FieldOffset(0)]
            public short mdi_rec_type;   /* record type */
            [FieldOffset(2)]
            public byte mdi_key_code;   /* key code */
            [FieldOffset(3)]
            public byte mdi_pw_flag;    /* power on flag */
            [FieldOffset(4)]
            public sbyte mdi_dummy1;
            [FieldOffset(5)]
            public sbyte mdi_dummy2;
            [FieldOffset(6)]
            public sbyte mdi_dummy3;
            [FieldOffset(7)]
            public sbyte mdi_dummy4;

            // sign record
            [FieldOffset(0)]
            public short sgn_rec_type;   /* record type */
            [FieldOffset(2)]
            public sbyte sgn_sig_name;   /* signal name */
            [FieldOffset(3)]
            public byte sgn_sig_old;    /* old signal bit pattern */
            [FieldOffset(4)]
            public byte sgn_sig_new;    /* new signal bit pattern */
            [FieldOffset(5)]
            public sbyte sgn_dummy;
            [FieldOffset(6)]
            public short sgn_sig_no;     /* signal number */

            // date record
            [FieldOffset(0)]
            public short date_rec_type;   /* record type */
            [FieldOffset(2)]
            public sbyte date_year;       /* year */
            [FieldOffset(3)]
            public sbyte date_month;      /* month */
            [FieldOffset(4)]
            public sbyte date_day;        /* day */
            [FieldOffset(5)]
            public sbyte date_pw_flag;    /* power on flag */
            [FieldOffset(6)]
            public sbyte date_dummy1;
            [FieldOffset(7)]
            public sbyte date_dummy2;

            // time record
            [FieldOffset(0)]
            public short time_rec_type;   /* record flag */
            [FieldOffset(2)]
            public sbyte time_hour;       /* hour */
            [FieldOffset(3)]
            public sbyte time_minute;     /* minute */
            [FieldOffset(4)]
            public sbyte time_second;     /* second */
            [FieldOffset(5)]
            public sbyte time_pw_flag;    /* power on flag */
            [FieldOffset(6)]
            public sbyte time_dummy1;
            [FieldOffset(7)]
            public sbyte time_dummy2;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHIS1
        {
            public ODBHIS_data data1 = new ODBHIS_data();
            public ODBHIS_data data2 = new ODBHIS_data();
            public ODBHIS_data data3 = new ODBHIS_data();
            public ODBHIS_data data4 = new ODBHIS_data();
            public ODBHIS_data data5 = new ODBHIS_data();
            public ODBHIS_data data6 = new ODBHIS_data();
            public ODBHIS_data data7 = new ODBHIS_data();
            public ODBHIS_data data8 = new ODBHIS_data();
            public ODBHIS_data data9 = new ODBHIS_data();
            public ODBHIS_data data10 = new ODBHIS_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHIS
        {
            public ushort s_no;   /* start number */
            public short type;   /* dummy */
            public ushort e_no;   /* end number */
            public ODBHIS1 data = new ODBHIS1();
        }

        /* cnc_rdophistry2:read operation history data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MDI2
        {
            public byte key_code;   /* key code */
            public byte pw_flag;    /* power on flag */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MDI2_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_MDI2 data = new REC_MDI2();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_SGN2
        {
            public short sig_name;   /* signal name */
            public short sig_no;     /* signal number */
            public byte sig_old;    /* old signal bit pattern */
            public byte sig_new;    /* new signal bit pattern */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_SGN2_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_SGN2 data = new REC_SGN2();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_ALM2
        {
            public short alm_grp;    /* alarm group */
            public short alm_no;     /* alarm number */
            public short axis_no;    /* axis number */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_ALM2_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_ALM2 data = new REC_ALM2();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_DATE2
        {
            public short evnt_type;  /* event type */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_DATE2_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_DATE2 data = new REC_DATE2();
        }
        [StructLayout(LayoutKind.Explicit)]
        public class ODBOPHIS
        {
            [FieldOffset(0)]
            public REC_MDI2_data rec_mdi = new REC_MDI2_data();
            [FieldOffset(0)]
            public REC_SGN2_data rec_sgn = new REC_SGN2_data();
            [FieldOffset(0)]
            public REC_ALM2_data rec_alm = new REC_ALM2_data();
            [FieldOffset(0)]
            public REC_DATE2_data rec_date = new REC_DATE2_data();
        }

        /* cnc_rdophistry4:read operation history data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MDI4
        {
            public char key_code;   /* key code */
            public char pw_flag;    /* power on flag */
            public short pth_no;     /* path index */
            public short ex_flag;    /* kxternal key flag */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MDI4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_MDI4 data = new REC_MDI4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_SGN4
        {
            public short sig_name;   /* signal name */
            public short sig_no;     /* signal number */
            public char sig_old;    /* old signal bit pattern */
            public char sig_new;    /* new signal bit pattern */
            public short pmc_no;     /* pmc index */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_SGN4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_SGN4 data = new REC_SGN4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_ALM4
        {
            public short alm_grp;    /* alarm group */
            public short alm_no;     /* alarm number */
            public short axis_no;    /* axis number */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_ALM4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_ALM4 data = new REC_ALM4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_DATE4
        {
            public short evnt_type;  /* event type */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_DATE4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_DATE4 data = new REC_DATE4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_IAL4
        {
            public short alm_grp;    /* alarm group */
            public short alm_no;     /* alarm number */
            public short axis_no;    /* axis number */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
            public short sys_alm;    /* sys alarm */
            public short dsp_flg;    /* message dsp flag */
            public short axis_num;   /* axis num */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] g_modal;    /* G code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] g_dp;       /* #7:1 Block */
            /* #6乣#0 dp*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] a_modal;    /* B,D,E,F,H,M,N,O,S,T code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] a_dp;       /* #7:1 Block */
            /* 6乣#0 dp*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] abs_pos;    /* Abs pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] abs_dp;     /* Abs dp  */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] mcn_pos;    /* Mcn pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] mcn_dp;     /* Mcn dp  */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_IAL4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_IAL4 data = new REC_IAL4();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class REC_MAL4
        {
            public short alm_grp;    /* alarm group */
            public short alm_no;     /* alarm number */
            public short axis_no;    /* axis number */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
            public short sys_alm;    /* sys alarm */
            public short dsp_flg;    /* message dsp flag */
            public short axis_num;   /* axis num */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string alm_msg = new string(' ', 64);  /* alarm message */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] g_modal;    /* G code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] g_dp;       /* #7:1 Block */
            /* #6乣#0 dp*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] a_modal;    /* B,D,E,F,H,M,N,O,S,T code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] a_dp;       /* #7:1 Block */
            /* 6乣#0 dp*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] abs_pos;    /* Abs pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] abs_dp;     /* Abs dp  */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] mcn_pos;    /* Mcn pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] mcn_dp;     /* Mcn dp  */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MAL4_data
        {
            public short rec_len;     /* length */
            public short rec_type;    /* record type */
            public REC_MAL4 data = new REC_MAL4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_OPM4
        {
            public short dsp_flg;     /* Dysplay flag(ON/OFF) */
            public short om_no;       /* message number */
            public short year;        /* year */
            public short month;       /* month */
            public short day;         /* day */
            public short hour;        /* Hour */
            public short minute;      /* Minute */
            public short second;      /* Second */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string ope_msg = new string(' ', 256);  /* Messege */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_OPM4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_OPM4 data = new REC_OPM4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_OFS4
        {
            public short ofs_grp;    /* Tool offset group */
            public short ofs_no;     /* Tool offset number */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
            public int ofs_old;    /* old data */
            public int ofs_new;    /* new data */
            public short old_dp;     /* old data decimal point */
            public short new_dp;     /* new data decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_OFS4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_OFS4 data = new REC_OFS4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_PRM4
        {
            public short prm_grp;    /* paramater group */
            public short prm_num;    /* paramater number */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short prm_len;    /* paramater data length */
            public int prm_no;     /* paramater no */
            public int prm_old;    /* old data */
            public int prm_new;    /* new data */
            public short old_dp;     /* old data decimal point */
            public short new_dp;     /* new data decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_PRM4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_PRM4 data = new REC_PRM4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_WOF4
        {
            public short ofs_grp;    /* Work offset group */
            public short ofs_no;     /* Work offset number */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
            public short axis_no;    /* path axis num $*/
            public short dummy;
            public int ofs_old;    /* old data */
            public int ofs_new;    /* new data */
            public short old_dp;     /* old data decimal point */
            public short new_dp;     /* new data decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_WOF4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_WOF4 data = new REC_WOF4();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MAC4
        {
            public short mac_no;     /* macro val number */
            public short hour;       /* hour */
            public short minute;     /* minute */
            public short second;     /* second */
            public short pth_no;     /* path index */
            public int mac_old;    /* old data */
            public int mac_new;    /* new data */
            public short old_dp;     /* old data decimal point */
            public short new_dp;     /* old data decimal point */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REC_MAC4_data
        {
            public short rec_len;    /* length */
            public short rec_type;   /* record type */
            public REC_MAC4 data = new REC_MAC4();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_1
        {
            public REC_MDI4_data rec_mdi1 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi2 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi3 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi4 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi5 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi6 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi7 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi8 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi9 = new REC_MDI4_data();
            public REC_MDI4_data rec_mdi10 = new REC_MDI4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_2
        {
            public REC_SGN4_data rec_sgn1 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn2 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn3 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn4 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn5 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn6 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn7 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn8 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn9 = new REC_SGN4_data();
            public REC_SGN4_data rec_sgn10 = new REC_SGN4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_3
        {
            public REC_ALM4_data rec_alm1 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm2 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm3 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm4 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm5 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm6 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm7 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm8 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm9 = new REC_ALM4_data();
            public REC_ALM4_data rec_alm10 = new REC_ALM4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_4
        {
            public REC_DATE4_data rec_date1 = new REC_DATE4_data();
            public REC_DATE4_data rec_date2 = new REC_DATE4_data();
            public REC_DATE4_data rec_date3 = new REC_DATE4_data();
            public REC_DATE4_data rec_date4 = new REC_DATE4_data();
            public REC_DATE4_data rec_date5 = new REC_DATE4_data();
            public REC_DATE4_data rec_date6 = new REC_DATE4_data();
            public REC_DATE4_data rec_date7 = new REC_DATE4_data();
            public REC_DATE4_data rec_date8 = new REC_DATE4_data();
            public REC_DATE4_data rec_date9 = new REC_DATE4_data();
            public REC_DATE4_data rec_date10 = new REC_DATE4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_5
        {
            public REC_IAL4_data rec_ial1 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial2 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial3 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial4 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial5 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial6 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial7 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial8 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial9 = new REC_IAL4_data();
            public REC_IAL4_data rec_ial10 = new REC_IAL4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_6
        {
            public REC_MAL4_data rec_mal1 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal2 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal3 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal4 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal5 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal6 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal7 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal8 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal9 = new REC_MAL4_data();
            public REC_MAL4_data rec_mal10 = new REC_MAL4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_7
        {
            public REC_OPM4_data rec_opm1 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm2 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm3 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm4 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm5 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm6 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm7 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm8 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm9 = new REC_OPM4_data();
            public REC_OPM4_data rec_opm10 = new REC_OPM4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_8
        {
            public REC_OFS4_data rec_ofs1 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs2 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs3 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs4 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs5 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs6 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs7 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs8 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs9 = new REC_OFS4_data();
            public REC_OFS4_data rec_ofs10 = new REC_OFS4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_9
        {
            public REC_PRM4_data rec_prm1 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm2 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm3 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm4 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm5 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm6 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm7 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm8 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm9 = new REC_PRM4_data();
            public REC_PRM4_data rec_prm10 = new REC_PRM4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_10
        {
            public REC_WOF4_data rec_wof1 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof2 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof3 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof4 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof5 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof6 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof7 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof8 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof9 = new REC_WOF4_data();
            public REC_WOF4_data rec_wof10 = new REC_WOF4_data();
        } /* In case that the number of data is 10 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOPHIS4_11
        {
            public REC_MAC4_data rec_mac1 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac2 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac3 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac4 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac5 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac6 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac7 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac8 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac9 = new REC_MAC4_data();
            public REC_MAC4_data rec_mac10 = new REC_MAC4_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdalmhistry:read alarm history data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ALM_HIS_data
        {
            public short dummy;
            public short alm_grp;        /* alarm group */
            public short alm_no;         /* alarm number */
            public byte axis_no;        /* axis number */
            public byte year;           /* year */
            public byte month;          /* month */
            public byte day;            /* day */
            public byte hour;           /* hour */
            public byte minute;         /* minute */
            public byte second;         /* second */
            public byte dummy2;
            public short len_msg;        /* alarm message length */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string alm_msg = new string(' ', 32);  /* alarm message */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALM_HIS1
        {
            public ALM_HIS_data data1 = new ALM_HIS_data();
            public ALM_HIS_data data2 = new ALM_HIS_data();
            public ALM_HIS_data data3 = new ALM_HIS_data();
            public ALM_HIS_data data4 = new ALM_HIS_data();
            public ALM_HIS_data data5 = new ALM_HIS_data();
            public ALM_HIS_data data6 = new ALM_HIS_data();
            public ALM_HIS_data data7 = new ALM_HIS_data();
            public ALM_HIS_data data8 = new ALM_HIS_data();
            public ALM_HIS_data data9 = new ALM_HIS_data();
            public ALM_HIS_data data10 = new ALM_HIS_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAHIS
        {
            public ushort s_no;   /* start number */
            public short type;   /* dummy */
            public ushort e_no;   /* end number */
            public ALM_HIS1 alm_his = new ALM_HIS1();
        }

        /* cnc_rdalmhistry2:read alarm history data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ALM_HIS2_data
        {
            public short alm_grp;        /* alarm group */
            public short alm_no;         /* alarm number */
            public short axis_no;        /* axis number */
            public short year;           /* year */
            public short month;          /* month */
            public short day;            /* day */
            public short hour;           /* hour */
            public short minute;         /* minute */
            public short second;         /* second */
            public short len_msg;        /* alarm message length */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string alm_msg = new string(' ', 32);  /* alarm message */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALM_HIS2
        {
            public ALM_HIS2_data data1 = new ALM_HIS2_data();
            public ALM_HIS2_data data2 = new ALM_HIS2_data();
            public ALM_HIS2_data data3 = new ALM_HIS2_data();
            public ALM_HIS2_data data4 = new ALM_HIS2_data();
            public ALM_HIS2_data data5 = new ALM_HIS2_data();
            public ALM_HIS2_data data6 = new ALM_HIS2_data();
            public ALM_HIS2_data data7 = new ALM_HIS2_data();
            public ALM_HIS2_data data8 = new ALM_HIS2_data();
            public ALM_HIS2_data data9 = new ALM_HIS2_data();
            public ALM_HIS2_data data10 = new ALM_HIS2_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAHIS2
        {
            public ushort s_no;   /* start number */
            public ushort e_no;   /* end number */
            public ALM_HIS2 alm_his = new ALM_HIS2();
        }

        /* cnc_rdalmhistry3:read alarm history data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ALM_HIS3_data
        {
            public short alm_grp;        /* alarm group */
            public short alm_no;         /* alarm number */
            public short axis_no;        /* axis number */
            public short year;           /* year */
            public short month;          /* month */
            public short day;            /* day */
            public short hour;           /* hour */
            public short minute;         /* minute */
            public short second;         /* second */
            public short len_msg;        /* alarm message length */
            public short pth_no;         /* path index */
            public short dummy;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string alm_msg = new string(' ', 32);  /* alarm message */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALM_HIS3
        {
            public ALM_HIS3_data data1 = new ALM_HIS3_data();
            public ALM_HIS3_data data2 = new ALM_HIS3_data();
            public ALM_HIS3_data data3 = new ALM_HIS3_data();
            public ALM_HIS3_data data4 = new ALM_HIS3_data();
            public ALM_HIS3_data data5 = new ALM_HIS3_data();
            public ALM_HIS3_data data6 = new ALM_HIS3_data();
            public ALM_HIS3_data data7 = new ALM_HIS3_data();
            public ALM_HIS3_data data8 = new ALM_HIS3_data();
            public ALM_HIS3_data data9 = new ALM_HIS3_data();
            public ALM_HIS3_data data10 = new ALM_HIS3_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAHIS3
        {
            public ushort s_no;   /* start number */
            public ushort e_no;   /* end number */
            public ALM_HIS3 alm_his = new ALM_HIS3();
        }

        /* cnc_rdalmhistry5:read alarm history data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ALM_HIS5_data
        {
            public short alm_grp;        /* alarm group */
            public short alm_no;         /* alarm number */
            public short axis_no;        /* axis number */
            public short year;           /* year */
            public short month;          /* month */
            public short day;            /* day */
            public short hour;           /* hour */
            public short minute;         /* minute */
            public short second;         /* second */
            public short len_msg;        /* alarm message length */
            public short pth_no;         /* path index */
            public short dummy;          /* dummy */
            public short dsp_flg;        /* Flag for displaying  */
            public short axis_num;       /* Total axis number */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string alm_msg = new string(' ', 64);  /* alarm message */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] g_modal;        /* G code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] g_dp;           /* #7:1 Block  #6乣#0 dp */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] a_modal;        /* B,D,E,F,H,M,N,O,S,T code Modal */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] a_dp;           /* #7:1 Block  #6乣#0 dp */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] abs_pos;        /* Abs pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] abs_dp;         /* Abs dp */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] mcn_pos;        /* Mcn pos */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] mcn_dp;         /* Mcn dp */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALM_HIS5
        {
            public ALM_HIS5_data data1 = new ALM_HIS5_data();
            public ALM_HIS5_data data2 = new ALM_HIS5_data();
            public ALM_HIS5_data data3 = new ALM_HIS5_data();
            public ALM_HIS5_data data4 = new ALM_HIS5_data();
            public ALM_HIS5_data data5 = new ALM_HIS5_data();
            public ALM_HIS5_data data6 = new ALM_HIS5_data();
            public ALM_HIS5_data data7 = new ALM_HIS5_data();
            public ALM_HIS5_data data8 = new ALM_HIS5_data();
            public ALM_HIS5_data data9 = new ALM_HIS5_data();
            public ALM_HIS5_data data10 = new ALM_HIS5_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAHIS5
        {
            public ushort s_no;   /* start number */
            public ushort e_no;   /* end number */
            public ALM_HIS5 alm_his = new ALM_HIS5();
        }

        /* cnc_rdomhistry2:read operater message history data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBOMHIS2_data
        {
            public short dsp_flg;     /* Dysplay flag(ON/OFF) */
            public short om_no;       /* operater message number */
            public short year;        /* year */
            public short month;       /* month */
            public short day;         /* day */
            public short hour;        /* Hour */
            public short minute;      /* Minute */
            public short second;      /* Second */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string alm_msg = new string(' ', 256);  /* alarm message */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OPM_HIS
        {
            public ODBOMHIS2_data data1 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data2 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data3 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data4 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data5 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data6 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data7 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data8 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data9 = new ODBOMHIS2_data();
            public ODBOMHIS2_data data10 = new ODBOMHIS2_data();
        }  /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOMHIS2
        {
            public ushort s_no;   /* start number */
            public ushort e_no;   /* end number */
            public OPM_HIS opm_his = new OPM_HIS();
        }

        /* cnc_rdhissgnl:read signals related operation history */
        /* cnc_wrhissgnl:write signals related operation history */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBSIG_data
        {
            public short ent_no;     /* entry number */
            public short sig_no;     /* signal number */
            public byte sig_name;   /* signal name */
            public byte mask_pat;   /* signal mask pattern */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG1
        {
            public IODBSIG_data data1 = new IODBSIG_data();
            public IODBSIG_data data2 = new IODBSIG_data();
            public IODBSIG_data data3 = new IODBSIG_data();
            public IODBSIG_data data4 = new IODBSIG_data();
            public IODBSIG_data data5 = new IODBSIG_data();
            public IODBSIG_data data6 = new IODBSIG_data();
            public IODBSIG_data data7 = new IODBSIG_data();
            public IODBSIG_data data8 = new IODBSIG_data();
            public IODBSIG_data data9 = new IODBSIG_data();
            public IODBSIG_data data10 = new IODBSIG_data();
            public IODBSIG_data data11 = new IODBSIG_data();
            public IODBSIG_data data12 = new IODBSIG_data();
            public IODBSIG_data data13 = new IODBSIG_data();
            public IODBSIG_data data14 = new IODBSIG_data();
            public IODBSIG_data data15 = new IODBSIG_data();
            public IODBSIG_data data16 = new IODBSIG_data();
            public IODBSIG_data data17 = new IODBSIG_data();
            public IODBSIG_data data18 = new IODBSIG_data();
            public IODBSIG_data data19 = new IODBSIG_data();
            public IODBSIG_data data20 = new IODBSIG_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG
        {
            public short datano; /* dummy */
            public short type;   /* dummy */
            public IODBSIG1 data = new IODBSIG1();
        }

        /* cnc_rdhissgnl2:read signals related operation history 2 */
        /* cnc_wrhissgnl2:write signals related operation history 2 */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class _IODBSIG2_data
        {
            public short ent_no;     /* entry number */
            public short sig_no;     /* signal number */
            public byte sig_name;   /* signal name */
            public byte mask_pat;   /* signal mask pattern */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG2_data
        {
            public _IODBSIG2_data data1 = new _IODBSIG2_data();
            public _IODBSIG2_data data2 = new _IODBSIG2_data();
            public _IODBSIG2_data data3 = new _IODBSIG2_data();
            public _IODBSIG2_data data4 = new _IODBSIG2_data();
            public _IODBSIG2_data data5 = new _IODBSIG2_data();
            public _IODBSIG2_data data6 = new _IODBSIG2_data();
            public _IODBSIG2_data data7 = new _IODBSIG2_data();
            public _IODBSIG2_data data8 = new _IODBSIG2_data();
            public _IODBSIG2_data data9 = new _IODBSIG2_data();
            public _IODBSIG2_data data10 = new _IODBSIG2_data();
            public _IODBSIG2_data data11 = new _IODBSIG2_data();
            public _IODBSIG2_data data12 = new _IODBSIG2_data();
            public _IODBSIG2_data data13 = new _IODBSIG2_data();
            public _IODBSIG2_data data14 = new _IODBSIG2_data();
            public _IODBSIG2_data data15 = new _IODBSIG2_data();
            public _IODBSIG2_data data16 = new _IODBSIG2_data();
            public _IODBSIG2_data data17 = new _IODBSIG2_data();
            public _IODBSIG2_data data18 = new _IODBSIG2_data();
            public _IODBSIG2_data data19 = new _IODBSIG2_data();
            public _IODBSIG2_data data20 = new _IODBSIG2_data();
            public _IODBSIG2_data data31 = new _IODBSIG2_data();
            public _IODBSIG2_data data32 = new _IODBSIG2_data();
            public _IODBSIG2_data data33 = new _IODBSIG2_data();
            public _IODBSIG2_data data34 = new _IODBSIG2_data();
            public _IODBSIG2_data data35 = new _IODBSIG2_data();
            public _IODBSIG2_data data36 = new _IODBSIG2_data();
            public _IODBSIG2_data data37 = new _IODBSIG2_data();
            public _IODBSIG2_data data38 = new _IODBSIG2_data();
            public _IODBSIG2_data data39 = new _IODBSIG2_data();
            public _IODBSIG2_data data40 = new _IODBSIG2_data();
            public _IODBSIG2_data data41 = new _IODBSIG2_data();
            public _IODBSIG2_data data42 = new _IODBSIG2_data();
            public _IODBSIG2_data data43 = new _IODBSIG2_data();
            public _IODBSIG2_data data44 = new _IODBSIG2_data();
            public _IODBSIG2_data data45 = new _IODBSIG2_data();

        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG2
        {
            public short datano; /* dummy */
            public short type;   /* dummy */
            public IODBSIG2_data data = new IODBSIG2_data();
        }

        /* cnc_rdhissgnl3:read signals related operation history */
        /* cnc_wrhissgnl3:write signals related operation history */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class _IODBSIG3_data
        {
            public short ent_no;     /* entry number */
            public short pmc_no;     /* pmc number */
            public short sig_no;     /* signal number */
            public byte sig_name;   /* signal name */
            public byte mask_pat;   /* signal mask pattern */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG3_data
        {
            public _IODBSIG3_data data1 = new _IODBSIG3_data();
            public _IODBSIG3_data data2 = new _IODBSIG3_data();
            public _IODBSIG3_data data3 = new _IODBSIG3_data();
            public _IODBSIG3_data data4 = new _IODBSIG3_data();
            public _IODBSIG3_data data5 = new _IODBSIG3_data();
            public _IODBSIG3_data data6 = new _IODBSIG3_data();
            public _IODBSIG3_data data7 = new _IODBSIG3_data();
            public _IODBSIG3_data data8 = new _IODBSIG3_data();
            public _IODBSIG3_data data9 = new _IODBSIG3_data();
            public _IODBSIG3_data data10 = new _IODBSIG3_data();
            public _IODBSIG3_data data11 = new _IODBSIG3_data();
            public _IODBSIG3_data data12 = new _IODBSIG3_data();
            public _IODBSIG3_data data13 = new _IODBSIG3_data();
            public _IODBSIG3_data data14 = new _IODBSIG3_data();
            public _IODBSIG3_data data15 = new _IODBSIG3_data();
            public _IODBSIG3_data data16 = new _IODBSIG3_data();
            public _IODBSIG3_data data17 = new _IODBSIG3_data();
            public _IODBSIG3_data data18 = new _IODBSIG3_data();
            public _IODBSIG3_data data19 = new _IODBSIG3_data();
            public _IODBSIG3_data data20 = new _IODBSIG3_data();
            public _IODBSIG3_data data21 = new _IODBSIG3_data();
            public _IODBSIG3_data data22 = new _IODBSIG3_data();
            public _IODBSIG3_data data23 = new _IODBSIG3_data();
            public _IODBSIG3_data data24 = new _IODBSIG3_data();
            public _IODBSIG3_data data25 = new _IODBSIG3_data();
            public _IODBSIG3_data data26 = new _IODBSIG3_data();
            public _IODBSIG3_data data27 = new _IODBSIG3_data();
            public _IODBSIG3_data data28 = new _IODBSIG3_data();
            public _IODBSIG3_data data29 = new _IODBSIG3_data();
            public _IODBSIG3_data data30 = new _IODBSIG3_data();
            public _IODBSIG3_data data31 = new _IODBSIG3_data();
            public _IODBSIG3_data data32 = new _IODBSIG3_data();
            public _IODBSIG3_data data33 = new _IODBSIG3_data();
            public _IODBSIG3_data data34 = new _IODBSIG3_data();
            public _IODBSIG3_data data35 = new _IODBSIG3_data();
            public _IODBSIG3_data data36 = new _IODBSIG3_data();
            public _IODBSIG3_data data37 = new _IODBSIG3_data();
            public _IODBSIG3_data data38 = new _IODBSIG3_data();
            public _IODBSIG3_data data39 = new _IODBSIG3_data();
            public _IODBSIG3_data data40 = new _IODBSIG3_data();
            public _IODBSIG3_data data41 = new _IODBSIG3_data();
            public _IODBSIG3_data data42 = new _IODBSIG3_data();
            public _IODBSIG3_data data43 = new _IODBSIG3_data();
            public _IODBSIG3_data data44 = new _IODBSIG3_data();
            public _IODBSIG3_data data45 = new _IODBSIG3_data();
            public _IODBSIG3_data data46 = new _IODBSIG3_data();
            public _IODBSIG3_data data47 = new _IODBSIG3_data();
            public _IODBSIG3_data data48 = new _IODBSIG3_data();
            public _IODBSIG3_data data49 = new _IODBSIG3_data();
            public _IODBSIG3_data data50 = new _IODBSIG3_data();
            public _IODBSIG3_data data51 = new _IODBSIG3_data();
            public _IODBSIG3_data data52 = new _IODBSIG3_data();
            public _IODBSIG3_data data53 = new _IODBSIG3_data();
            public _IODBSIG3_data data54 = new _IODBSIG3_data();
            public _IODBSIG3_data data55 = new _IODBSIG3_data();
            public _IODBSIG3_data data56 = new _IODBSIG3_data();
            public _IODBSIG3_data data57 = new _IODBSIG3_data();
            public _IODBSIG3_data data58 = new _IODBSIG3_data();
            public _IODBSIG3_data data59 = new _IODBSIG3_data();
            public _IODBSIG3_data data60 = new _IODBSIG3_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIG3
        {
            public short datano; /* dummy */
            public short type;   /* dummy */
            public IODBSIG3_data data = new IODBSIG3_data();
        }

        /*-------------*/
        /* CNC: Others */
        /*-------------*/

        /* cnc_sysinfo:read CNC system information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYS
        {
            public short addinfo;
            public short max_axis;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] cnc_type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] mt_type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] series;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] axes;
        }

#if FS15D
    /* cnc_statinfo:read CNC status information */
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBST
    {
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public short[] dummy;      /* dummy                    */
        public short   aut;        /* selected automatic mode  */
        public short   manual;     /* selected manual mode     */
        public short   run;        /* running status           */
        public short   edit;       /* editting status          */
        public short   motion;     /* axis, dwell status       */
        public short   mstb;       /* m, s, t, b status        */
        public short   emergency;  /* emergency stop status    */
        public short   write;      /* writting status          */
        public short   labelskip;  /* label skip status        */
        public short   alarm;      /* alarm status             */
        public short   warning;    /* warning status           */
        public short   battery;    /* battery status           */
    }
#else
        /* cnc_statinfo:read CNC status information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBST
        {
            public short dummy;     /* dummy */
            public short tmmode;    /* T/M mode */
            public short aut;       /* selected automatic mode */
            public short run;       /* running status */
            public short motion;    /* axis, dwell status */
            public short mstb;      /* m, s, t, b status */
            public short emergency; /* emergency stop status */
            public short alarm;     /* alarm status */
            public short edit;      /* editting status */
        }
#endif

        /* cnc_alarm:read alarm status */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBALM
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] dummy = { 0, 0 };
            public ushort data = 0;
        }

        /* cnc_rdalminfo:read alarm information */
#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ALMINFO1_data
    {
        public int     axis;
        public short   alm_no;
    }
    
    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi,Pack=4)]
    public class ALMINFO2_data
    {
        public int     axis=0 ;
        public short   alm_no=0 ;
        public short   msg_len=0 ;
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
        public string  alm_msg= new string(' ',32) ;
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALMINFO1_data
        {
            public short axis;
            public short alm_no;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ALMINFO2_data
        {
            public short axis = 0;
            public short alm_no = 0;
            public short msg_len = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string alm_msg = new string(' ', 32);
        }
#endif
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALMINFO_1
        {
            public ALMINFO1_data msg1 = new ALMINFO1_data();
            public ALMINFO1_data msg2 = new ALMINFO1_data();
            public ALMINFO1_data msg3 = new ALMINFO1_data();
            public ALMINFO1_data msg4 = new ALMINFO1_data();
            public ALMINFO1_data msg5 = new ALMINFO1_data();
            public short data_end;
        } /* In case that the number of alarm is 5 */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ALMINFO_2
        {
            public ALMINFO2_data msg1 = new ALMINFO2_data();
            public ALMINFO2_data msg2 = new ALMINFO2_data();
            public ALMINFO2_data msg3 = new ALMINFO2_data();
            public ALMINFO2_data msg4 = new ALMINFO2_data();
            public ALMINFO2_data msg5 = new ALMINFO2_data();
            public short dataend = 0;
        } /* In case that the number of alarm is 5 */

        /* cnc_rdalmmsg:read alarm messages */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBALMMSG_data
        {
            public int alm_no;
            public short type;
            public short axis;
            public short dummy;
            public short msg_len;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string alm_msg = new string(' ', 32);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBALMMSG
        {
            public ODBALMMSG_data msg1 = new ODBALMMSG_data();
            public ODBALMMSG_data msg2 = new ODBALMMSG_data();
            public ODBALMMSG_data msg3 = new ODBALMMSG_data();
            public ODBALMMSG_data msg4 = new ODBALMMSG_data();
            public ODBALMMSG_data msg5 = new ODBALMMSG_data();
            public ODBALMMSG_data msg6 = new ODBALMMSG_data();
            public ODBALMMSG_data msg7 = new ODBALMMSG_data();
            public ODBALMMSG_data msg8 = new ODBALMMSG_data();
            public ODBALMMSG_data msg9 = new ODBALMMSG_data();
            public ODBALMMSG_data msg10 = new ODBALMMSG_data();
        } /* In case that the number of alarm is 10 */

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBALMMSG2_data
        {
            public int alm_no;
            public short type;
            public short axis;
            public short dummy;
            public short msg_len;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string alm_msg = new string(' ', 64);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBALMMSG2
        {
            public ODBALMMSG2_data msg1 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg2 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg3 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg4 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg5 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg6 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg7 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg8 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg9 = new ODBALMMSG2_data();
            public ODBALMMSG2_data msg10 = new ODBALMMSG2_data();
        } /* In case that the number of alarm is 10 */

        /* cnc_modal:read modal data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class MODAL_AUX_data
        {
            public int aux_data;
            public byte flag1;
            public byte flag2;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class MODAL_RAUX1_data
        {
            public MODAL_AUX_data data1 = new MODAL_AUX_data();
            public MODAL_AUX_data data2 = new MODAL_AUX_data();
            public MODAL_AUX_data data3 = new MODAL_AUX_data();
            public MODAL_AUX_data data4 = new MODAL_AUX_data();
            public MODAL_AUX_data data5 = new MODAL_AUX_data();
            public MODAL_AUX_data data6 = new MODAL_AUX_data();
            public MODAL_AUX_data data7 = new MODAL_AUX_data();
            public MODAL_AUX_data data8 = new MODAL_AUX_data();
            public MODAL_AUX_data data9 = new MODAL_AUX_data();
            public MODAL_AUX_data data10 = new MODAL_AUX_data();
            public MODAL_AUX_data data11 = new MODAL_AUX_data();
            public MODAL_AUX_data data12 = new MODAL_AUX_data();
            public MODAL_AUX_data data13 = new MODAL_AUX_data();
            public MODAL_AUX_data data14 = new MODAL_AUX_data();
            public MODAL_AUX_data data15 = new MODAL_AUX_data();
            public MODAL_AUX_data data16 = new MODAL_AUX_data();
            public MODAL_AUX_data data17 = new MODAL_AUX_data();
            public MODAL_AUX_data data18 = new MODAL_AUX_data();
            public MODAL_AUX_data data19 = new MODAL_AUX_data();
            public MODAL_AUX_data data20 = new MODAL_AUX_data();
            public MODAL_AUX_data data21 = new MODAL_AUX_data();
            public MODAL_AUX_data data22 = new MODAL_AUX_data();
            public MODAL_AUX_data data23 = new MODAL_AUX_data();
            public MODAL_AUX_data data24 = new MODAL_AUX_data();
            public MODAL_AUX_data data25 = new MODAL_AUX_data();
            public MODAL_AUX_data data26 = new MODAL_AUX_data();
            public MODAL_AUX_data data27 = new MODAL_AUX_data();
        }
#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class MODAL_RAUX2_data
    {
        public MODAL_AUX_data data1 = new MODAL_AUX_data();
        public MODAL_AUX_data data2 = new MODAL_AUX_data();
        public MODAL_AUX_data data3 = new MODAL_AUX_data();
        public MODAL_AUX_data data4 = new MODAL_AUX_data();
        public MODAL_AUX_data data5 = new MODAL_AUX_data();
        public MODAL_AUX_data data6 = new MODAL_AUX_data();
        public MODAL_AUX_data data7 = new MODAL_AUX_data();
        public MODAL_AUX_data data8 = new MODAL_AUX_data();
        public MODAL_AUX_data data9 = new MODAL_AUX_data();
        public MODAL_AUX_data data10= new MODAL_AUX_data();
        public MODAL_AUX_data data11= new MODAL_AUX_data();
        public MODAL_AUX_data data12= new MODAL_AUX_data();
        public MODAL_AUX_data data13= new MODAL_AUX_data();
        public MODAL_AUX_data data14= new MODAL_AUX_data();
        public MODAL_AUX_data data15= new MODAL_AUX_data();
        public MODAL_AUX_data data16= new MODAL_AUX_data();
        public MODAL_AUX_data data17= new MODAL_AUX_data();
        public MODAL_AUX_data data18= new MODAL_AUX_data();
        public MODAL_AUX_data data19= new MODAL_AUX_data();
        public MODAL_AUX_data data20= new MODAL_AUX_data();
        public MODAL_AUX_data data21= new MODAL_AUX_data();
        public MODAL_AUX_data data22= new MODAL_AUX_data();
        public MODAL_AUX_data data23= new MODAL_AUX_data();
        public MODAL_AUX_data data24= new MODAL_AUX_data();
    }
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class MODAL_RAUX2_data
    {
        public MODAL_AUX_data data1 = new MODAL_AUX_data();
        public MODAL_AUX_data data2 = new MODAL_AUX_data();
        public MODAL_AUX_data data3 = new MODAL_AUX_data();
        public MODAL_AUX_data data4 = new MODAL_AUX_data();
        public MODAL_AUX_data data5 = new MODAL_AUX_data();
        public MODAL_AUX_data data6 = new MODAL_AUX_data();
        public MODAL_AUX_data data7 = new MODAL_AUX_data();
        public MODAL_AUX_data data8 = new MODAL_AUX_data();
        public MODAL_AUX_data data9 = new MODAL_AUX_data();
        public MODAL_AUX_data data10= new MODAL_AUX_data();
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class MODAL_RAUX2_data
        {
            public MODAL_AUX_data data1 = new MODAL_AUX_data();
            public MODAL_AUX_data data2 = new MODAL_AUX_data();
            public MODAL_AUX_data data3 = new MODAL_AUX_data();
            public MODAL_AUX_data data4 = new MODAL_AUX_data();
            public MODAL_AUX_data data5 = new MODAL_AUX_data();
            public MODAL_AUX_data data6 = new MODAL_AUX_data();
            public MODAL_AUX_data data7 = new MODAL_AUX_data();
            public MODAL_AUX_data data8 = new MODAL_AUX_data();
        }
#endif

        [StructLayout(LayoutKind.Explicit)]
        public class ODBMDL_1
        {
            [FieldOffset(0)]
            public short datano;
            [FieldOffset(2)]
            public short type;
            [FieldOffset(4)]
            public byte g_data;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class ODBMDL_2
        {
            [FieldOffset(0)]
            public short datano;
            [FieldOffset(2)]
            public short type;
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] g_1shot = new byte[4];
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] g_rdata = new byte[35];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMDL_3
        {
            public short datano;
            public short type;
            public MODAL_AUX_data aux = new MODAL_AUX_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMDL_4
        {
            public short datano;
            public short type;
            public MODAL_RAUX1_data raux1 = new MODAL_RAUX1_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMDL_5
        {
            public short datano;
            public short type;
            public MODAL_RAUX2_data raux2 = new MODAL_RAUX2_data();
        }

        /* cnc_rdgcode: read G code */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBGCD_data
        {
            public short group;
            public short flag;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string code = new string(' ', 8);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBGCD
        {
            public ODBGCD_data gcd0 = new ODBGCD_data();
            public ODBGCD_data gcd1 = new ODBGCD_data();
            public ODBGCD_data gcd2 = new ODBGCD_data();
            public ODBGCD_data gcd3 = new ODBGCD_data();
            public ODBGCD_data gcd4 = new ODBGCD_data();
            public ODBGCD_data gcd5 = new ODBGCD_data();
            public ODBGCD_data gcd6 = new ODBGCD_data();
            public ODBGCD_data gcd7 = new ODBGCD_data();
            public ODBGCD_data gcd8 = new ODBGCD_data();
            public ODBGCD_data gcd9 = new ODBGCD_data();
            public ODBGCD_data gcd10 = new ODBGCD_data();
            public ODBGCD_data gcd11 = new ODBGCD_data();
            public ODBGCD_data gcd12 = new ODBGCD_data();
            public ODBGCD_data gcd13 = new ODBGCD_data();
            public ODBGCD_data gcd14 = new ODBGCD_data();
            public ODBGCD_data gcd15 = new ODBGCD_data();
            public ODBGCD_data gcd16 = new ODBGCD_data();
            public ODBGCD_data gcd17 = new ODBGCD_data();
            public ODBGCD_data gcd18 = new ODBGCD_data();
            public ODBGCD_data gcd19 = new ODBGCD_data();
            public ODBGCD_data gcd20 = new ODBGCD_data();
            public ODBGCD_data gcd21 = new ODBGCD_data();
            public ODBGCD_data gcd22 = new ODBGCD_data();
            public ODBGCD_data gcd23 = new ODBGCD_data();
            public ODBGCD_data gcd24 = new ODBGCD_data();
            public ODBGCD_data gcd25 = new ODBGCD_data();
            public ODBGCD_data gcd26 = new ODBGCD_data();
            public ODBGCD_data gcd27 = new ODBGCD_data();
        }

        /* cnc_rdcommand: read command value */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBCMD_data
        {
            public byte adrs;
            public byte num;
            public short flag;
            public int cmd_val;
            public int dec_val;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBCMD
        {
            public ODBCMD_data cmd0 = new ODBCMD_data();
            public ODBCMD_data cmd1 = new ODBCMD_data();
            public ODBCMD_data cmd2 = new ODBCMD_data();
            public ODBCMD_data cmd3 = new ODBCMD_data();
            public ODBCMD_data cmd4 = new ODBCMD_data();
            public ODBCMD_data cmd5 = new ODBCMD_data();
            public ODBCMD_data cmd6 = new ODBCMD_data();
            public ODBCMD_data cmd7 = new ODBCMD_data();
            public ODBCMD_data cmd8 = new ODBCMD_data();
            public ODBCMD_data cmd9 = new ODBCMD_data();
            public ODBCMD_data cmd10 = new ODBCMD_data();
            public ODBCMD_data cmd11 = new ODBCMD_data();
            public ODBCMD_data cmd12 = new ODBCMD_data();
            public ODBCMD_data cmd13 = new ODBCMD_data();
            public ODBCMD_data cmd14 = new ODBCMD_data();
            public ODBCMD_data cmd15 = new ODBCMD_data();
            public ODBCMD_data cmd16 = new ODBCMD_data();
            public ODBCMD_data cmd17 = new ODBCMD_data();
            public ODBCMD_data cmd18 = new ODBCMD_data();
            public ODBCMD_data cmd19 = new ODBCMD_data();
            public ODBCMD_data cmd20 = new ODBCMD_data();
            public ODBCMD_data cmd21 = new ODBCMD_data();
            public ODBCMD_data cmd22 = new ODBCMD_data();
            public ODBCMD_data cmd23 = new ODBCMD_data();
            public ODBCMD_data cmd24 = new ODBCMD_data();
            public ODBCMD_data cmd25 = new ODBCMD_data();
            public ODBCMD_data cmd26 = new ODBCMD_data();
            public ODBCMD_data cmd27 = new ODBCMD_data();
            public ODBCMD_data cmd28 = new ODBCMD_data();
            public ODBCMD_data cmd29 = new ODBCMD_data();
        }

        /* cnc_diagnoss:read diagnosis data */
        /* cnc_diagnosr:read diagnosis data(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REALDGN
        {
            public int dgn_val;     /* data of real diagnoss */
            public int dec_val;     /* decimal point of real diagnoss */
        }

#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class REALDGNS
    {
        public REALDGN rdata1=new REALDGN();
        public REALDGN rdata2=new REALDGN();
        public REALDGN rdata3=new REALDGN();
        public REALDGN rdata4=new REALDGN();
        public REALDGN rdata5=new REALDGN();
        public REALDGN rdata6=new REALDGN();
        public REALDGN rdata7=new REALDGN();
        public REALDGN rdata8=new REALDGN();
        public REALDGN rdata9=new REALDGN();
        public REALDGN rdata10=new REALDGN();
        public REALDGN rdata11=new REALDGN();
        public REALDGN rdata12=new REALDGN();
        public REALDGN rdata13=new REALDGN();
        public REALDGN rdata14=new REALDGN();
        public REALDGN rdata15=new REALDGN();
        public REALDGN rdata16=new REALDGN();
        public REALDGN rdata17=new REALDGN();
        public REALDGN rdata18=new REALDGN();
        public REALDGN rdata19=new REALDGN();
        public REALDGN rdata20=new REALDGN();
        public REALDGN rdata21=new REALDGN();
        public REALDGN rdata22=new REALDGN();
        public REALDGN rdata23=new REALDGN();
        public REALDGN rdata24=new REALDGN();
    } /* In case that the number of alarm is 24 */
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class REALDGNS
    {
        public REALDGN rdata1=new REALDGN();
        public REALDGN rdata2=new REALDGN();
        public REALDGN rdata3=new REALDGN();
        public REALDGN rdata4=new REALDGN();
        public REALDGN rdata5=new REALDGN();
        public REALDGN rdata6=new REALDGN();
        public REALDGN rdata7=new REALDGN();
        public REALDGN rdata8=new REALDGN();
        public REALDGN rdata9=new REALDGN();
        public REALDGN rdata10=new REALDGN();
    } /* In case that the number of alarm is 10 */
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class REALDGNS
        {
            public REALDGN rdata1 = new REALDGN();
            public REALDGN rdata2 = new REALDGN();
            public REALDGN rdata3 = new REALDGN();
            public REALDGN rdata4 = new REALDGN();
            public REALDGN rdata5 = new REALDGN();
            public REALDGN rdata6 = new REALDGN();
            public REALDGN rdata7 = new REALDGN();
            public REALDGN rdata8 = new REALDGN();
        } /* In case that the number of alarm is 8 */
#endif

        [StructLayout(LayoutKind.Explicit)]
        public class ODBDGN_1
        {
            [FieldOffset(0)]
            public short datano;    /* data number */
            [FieldOffset(2)]
            public short type;      /* axis number */
            [FieldOffset(4)]
            public byte cdata;     /* parameter / setting data */
            [FieldOffset(4)]
            public short idata;
            [FieldOffset(4)]
            public int ldata;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_2
        {
            public short datano;    /* data number */
            public short type;      /* axis number */
            public REALDGN rdata = new REALDGN();
        }
        [StructLayout(LayoutKind.Explicit)]
        public class ODBDGN_3
        {
            [FieldOffset(0)]
            public short datano;    /* data number */
            [FieldOffset(2)]
            public short type;      /* axis number */
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public byte[] cdatas;
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public short[] idatas;
            [FieldOffset(4),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ldatas;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_4
        {
            public short datano;    /* data number */
            public short type;      /* axis number */
            public REALDGNS rdatas = new REALDGNS();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_A
        {
            public ODBDGN_1 data1 = new ODBDGN_1();
            public ODBDGN_1 data2 = new ODBDGN_1();
            public ODBDGN_1 data3 = new ODBDGN_1();
            public ODBDGN_1 data4 = new ODBDGN_1();
            public ODBDGN_1 data5 = new ODBDGN_1();
            public ODBDGN_1 data6 = new ODBDGN_1();
            public ODBDGN_1 data7 = new ODBDGN_1();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_B
        {
            public ODBDGN_2 data1 = new ODBDGN_2();
            public ODBDGN_2 data2 = new ODBDGN_2();
            public ODBDGN_2 data3 = new ODBDGN_2();
            public ODBDGN_2 data4 = new ODBDGN_2();
            public ODBDGN_2 data5 = new ODBDGN_2();
            public ODBDGN_2 data6 = new ODBDGN_2();
            public ODBDGN_2 data7 = new ODBDGN_2();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_C
        {
            public ODBDGN_3 data1 = new ODBDGN_3();
            public ODBDGN_3 data2 = new ODBDGN_3();
            public ODBDGN_3 data3 = new ODBDGN_3();
            public ODBDGN_3 data4 = new ODBDGN_3();
            public ODBDGN_3 data5 = new ODBDGN_3();
            public ODBDGN_3 data6 = new ODBDGN_3();
            public ODBDGN_3 data7 = new ODBDGN_3();
        } /* (sample) must be modified */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDGN_D
        {
            public ODBDGN_4 data1 = new ODBDGN_4();
            public ODBDGN_4 data2 = new ODBDGN_4();
            public ODBDGN_4 data3 = new ODBDGN_4();
            public ODBDGN_4 data4 = new ODBDGN_4();
            public ODBDGN_4 data5 = new ODBDGN_4();
            public ODBDGN_4 data6 = new ODBDGN_4();
            public ODBDGN_4 data7 = new ODBDGN_4();
        } /* (sample) must be modified */

        /* cnc_adcnv:read A/D conversion data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAD
        {
            public short datano;    /* input analog voltage type */
            public short type;      /* analog voltage type */
            public short data;      /* digital voltage data */
        }

#if FS15D
    /* cnc_rdopmsg:read operator's message */
    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi,Pack=4)]
    public class OPMSG_data
    {
        public short datano ;     /* operator's message number */
        public short type ;       /* operator's message type   */
        public short char_num ;   /* message string length   */
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=129)]
        public string  data= new string(' ',129) ;  /* operator's message string */
    } /* In case that the data length is 129 */
#else
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class OPMSG_data
        {
            public short datano;     /* operator's message number */
            public short type;       /* operator's message type   */
            public short char_num;   /* message string length   */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string data = new string(' ', 256);  /* operator's message string */
        } /* In case that the data length is 256 */
#endif
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OPMSG
        {
            public OPMSG_data msg1 = new OPMSG_data();
            public OPMSG_data msg2 = new OPMSG_data();
            public OPMSG_data msg3 = new OPMSG_data();
            public OPMSG_data msg4 = new OPMSG_data();
            public OPMSG_data msg5 = new OPMSG_data();
        }

        /* cnc_rdopmsg2:read operator's message */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class OPMSG2_data
        {
            public short datano;    /* operator's message number */
            public short type;      /* operator's message type */
            public short char_num;  /* message string length */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string data = new string(' ', 64);  /* operator's message string */
        } /* In case that the data length is 64 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OPMSG2
        {
            public OPMSG2_data msg1 = new OPMSG2_data();
            public OPMSG2_data msg2 = new OPMSG2_data();
            public OPMSG2_data msg3 = new OPMSG2_data();
            public OPMSG2_data msg4 = new OPMSG2_data();
            public OPMSG2_data msg5 = new OPMSG2_data();
        }

        /* cnc_rdopmsg3:read operator's message */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class OPMSG3_data
        {
            public short datano;    /* operator's message number */
            public short type;      /* operator's message type */
            public short char_num;  /* message string length */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string data = new string(' ', 256);  /* operator's message string */
        } /* In case that the data length is 256 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class OPMSG3
        {
            public OPMSG3_data msg1 = new OPMSG3_data();
            public OPMSG3_data msg2 = new OPMSG3_data();
            public OPMSG3_data msg3 = new OPMSG3_data();
            public OPMSG3_data msg4 = new OPMSG3_data();
            public OPMSG3_data msg5 = new OPMSG3_data();
        }

        /* cnc_sysconfig:read CNC configuration information */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBSYSC
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_p;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_l;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] mod_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] soft_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_series16 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string s_version16 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] dummy;
            public short m_rom;
            public short s_rom;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] svo_soft;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] pmc_soft;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] lad_soft;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] mcr_soft;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] spl1_soft;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] spl2_soft;
            public short frmmin;
            public short drmmin;
            public short srmmin;
            public short pmcmin;
            public short crtmin;
            public short sv1min;
            public short sv3min;
            public short sicmin;
            public short posmin;
            public short drmmrc;
            public short drmarc;
            public short pmcmrc;
            public short dmaarc;
            public short iopt;
            public short hdiio;
            public short frmsub;
            public short drmsub;
            public short srmsub;
            public short sv5sub;
            public short sv7sub;
            public short sicsub;
            public short possub;
            public short hamsub;
            public short gm2gr1;
            public short crtgr2;
            public short gm1gr2;
            public short gm2gr2;
            public short cmmrb;
            public short sv5axs;
            public short sv7axs;
            public short sicaxs;
            public short posaxs;
            public short hanaxs;
            public short romr64;
            public short srmr64;
            public short dr1r64;
            public short dr2r64;
            public short iopio2;
            public short hdiio2;
            public short cmmrb2;
            public short romfap;
            public short srmfap;
            public short drmfap;
        }

        /* cnc_rdprstrinfo:read program restart information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPRS
        {
            public short datano;         /* dummy */
            public short type;           /* dummy */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] data_info;      /* data setting information */
            public int rstr_bc;        /* block counter */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public int[] rstr_m;         /* M code value */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] rstr_t;         /* T code value */
            public int rstr_s;         /* S code value */
            public int rstr_b;         /* B code value */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] dest;           /* program re-start position */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] dist;           /* program re-start distance */
        }

#if FS15D
    /* cnc_rdopnlsgnl:read output signal image of software operator's panel */
    /* cnc_wropnlsgnl:write output signal of software operator's panel */
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class IODBSGNL
    {
        public short   datano;     /* dummy */
        public short   type;       /* data select flag */
        public short   mode;       /* mode signal */
        public short   hndl_ax;    /* Manual handle feed axis selection signal */
        public short   hndl_mv;    /* Manual handle feed travel distance selection signal */
        public short   rpd_ovrd;   /* rapid traverse override signal */
        public short   jog_ovrd;   /* manual feedrate override signal */
        public short   feed_ovrd;  /* feedrate override signal */
        public short   spdl_ovrd;  /* spindle override signal */
        public short   blck_del;   /* optional block skip signal */
        public short   sngl_blck;  /* single block signal */
        public short   machn_lock; /* machine lock signal */
        public short   dry_run;    /* dry run signal */
        public short   mem_prtct;  /* memory protection signal */
        public short   feed_hold;  /* automatic operation halt signal */
        public short   manual_rpd; /* (not used) */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public short[] dummy;      /* (not used) */
    }
#else
        /* cnc_rdopnlsgnl:read output signal image of software operator's panel */
        /* cnc_wropnlsgnl:write output signal of software operator's panel */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSGNL
        {
            public short datano;     /* dummy */
            public short type;       /* data select flag */
            public short mode;       /* mode signal */
            public short hndl_ax;    /* Manual handle feed axis selection signal */
            public short hndl_mv;    /* Manual handle feed travel distance selection signal */
            public short rpd_ovrd;   /* rapid traverse override signal */
            public short jog_ovrd;   /* manual feedrate override signal */
            public short feed_ovrd;  /* feedrate override signal */
            public short spdl_ovrd;  /* (not used) */
            public short blck_del;   /* optional block skip signal */
            public short sngl_blck;  /* single block signal */
            public short machn_lock; /* machine lock signal */
            public short dry_run;    /* dry run signal */
            public short mem_prtct;  /* memory protection signal */
            public short feed_hold;  /* automatic operation halt signal */
        }
#endif

        /* cnc_rdopnlgnrl:read general signal image of software operator's panel */
        /* cnc_wropnlgnrl:write general signal image of software operator's panel */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBGNRL
        {
            public short datano; /* dummy */
            public short type;   /* data select flag */
            public byte sgnal;  /* general signal */
        }

        /* cnc_rdopnlgsname:read general signal name of software operator's panel */
        /* cnc_wropnlgsname:write general signal name of software operator's panel*/
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBRDNA
        {
            public short datano;         /* dummy */
            public short type;           /* data select flag */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl1_name = new string(' ', 9);  /* general signal 1 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl2_name = new string(' ', 9);  /* general signal 2 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl3_name = new string(' ', 9);  /* general signal 3 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl4_name = new string(' ', 9);  /* general signal 4 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl5_name = new string(' ', 9);  /* general signal 5 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl6_name = new string(' ', 9);  /* general signal 6 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl7_name = new string(' ', 9);  /* general signal 7 name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string sgnl8_name = new string(' ', 9);  /* general signal 8 name */
        }

        /* cnc_getdtailerr:get detail error */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBERR
        {
            public short err_no;
            public short err_dtno;
        }


        /* cnc_rdparainfo:read informations of CNC parameter */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPARAIF_info
        {
            public short prm_no;
            public short prm_type;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPARAIF1
        {
            public ODBPARAIF_info info1 = new ODBPARAIF_info();
            public ODBPARAIF_info info2 = new ODBPARAIF_info();
            public ODBPARAIF_info info3 = new ODBPARAIF_info();
            public ODBPARAIF_info info4 = new ODBPARAIF_info();
            public ODBPARAIF_info info5 = new ODBPARAIF_info();
            public ODBPARAIF_info info6 = new ODBPARAIF_info();
            public ODBPARAIF_info info7 = new ODBPARAIF_info();
            public ODBPARAIF_info info8 = new ODBPARAIF_info();
            public ODBPARAIF_info info9 = new ODBPARAIF_info();
            public ODBPARAIF_info info10 = new ODBPARAIF_info();
        }  /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPARAIF
        {
            public ushort info_no;
            public short prev_no;
            public short next_no;
            public ODBPARAIF1 info = new ODBPARAIF1();
        }

        /* cnc_rdsetinfo:read informations of CNC setting data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSETIF_info
        {
            public short set_no;
            public short set_type;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSETIF1
        {
            public ODBSETIF_info info1 = new ODBSETIF_info();
            public ODBSETIF_info info2 = new ODBSETIF_info();
            public ODBSETIF_info info3 = new ODBSETIF_info();
            public ODBSETIF_info info4 = new ODBSETIF_info();
            public ODBSETIF_info info5 = new ODBSETIF_info();
            public ODBSETIF_info info6 = new ODBSETIF_info();
            public ODBSETIF_info info7 = new ODBSETIF_info();
            public ODBSETIF_info info8 = new ODBSETIF_info();
            public ODBSETIF_info info9 = new ODBSETIF_info();
            public ODBSETIF_info info10 = new ODBSETIF_info();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSETIF
        {
            public ushort info_no;
            public short prev_no;
            public short next_no;
            public ODBSETIF1 info = new ODBSETIF1();
        }

        /* cnc_rddiaginfo:read informations of CNC diagnose data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDIAGIF_info
        {
            public short diag_no;
            public short diag_type;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDIAGIF1
        {
            public ODBDIAGIF_info info1 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info2 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info3 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info4 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info5 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info6 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info7 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info8 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info9 = new ODBDIAGIF_info();
            public ODBDIAGIF_info info10 = new ODBDIAGIF_info();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDIAGIF
        {
            public ushort info_no;
            public short prev_no;
            public short next_no;
            public ODBDIAGIF1 info = new ODBDIAGIF1();
        }

        /* cnc_rdparanum:read maximum, minimum and total number of CNC parameter */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPARANUM
        {
            public ushort para_min;
            public ushort para_max;
            public ushort total_no;
        }

        /* cnc_rdsetnum:read maximum, minimum and total number of CNC setting data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSETNUM
        {
            public ushort set_min;
            public ushort set_max;
            public ushort total_no;
        }

        /* cnc_rddiagnum:read maximum, minimum and total number of CNC diagnose data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDIAGNUM
        {
            public ushort diag_min;
            public ushort diag_max;
            public ushort total_no;
        }

        /* cnc_rdfrominfo:read F-ROM information on CNC  */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBFINFO_info
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string sysname = new string(' ', 12);  /* F-ROM SYSTEM data Name */
            public int fromsize;      /* F-ROM Size */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBFINFO1
        {
            public ODBFINFO_info info1 = new ODBFINFO_info();
            public ODBFINFO_info info2 = new ODBFINFO_info();
            public ODBFINFO_info info3 = new ODBFINFO_info();
            public ODBFINFO_info info4 = new ODBFINFO_info();
            public ODBFINFO_info info5 = new ODBFINFO_info();
            public ODBFINFO_info info6 = new ODBFINFO_info();
            public ODBFINFO_info info7 = new ODBFINFO_info();
            public ODBFINFO_info info8 = new ODBFINFO_info();
            public ODBFINFO_info info9 = new ODBFINFO_info();
            public ODBFINFO_info info10 = new ODBFINFO_info();
            public ODBFINFO_info info11 = new ODBFINFO_info();
            public ODBFINFO_info info12 = new ODBFINFO_info();
            public ODBFINFO_info info13 = new ODBFINFO_info();
            public ODBFINFO_info info14 = new ODBFINFO_info();
            public ODBFINFO_info info15 = new ODBFINFO_info();
            public ODBFINFO_info info16 = new ODBFINFO_info();
            public ODBFINFO_info info17 = new ODBFINFO_info();
            public ODBFINFO_info info18 = new ODBFINFO_info();
            public ODBFINFO_info info19 = new ODBFINFO_info();
            public ODBFINFO_info info20 = new ODBFINFO_info();
            public ODBFINFO_info info21 = new ODBFINFO_info();
            public ODBFINFO_info info22 = new ODBFINFO_info();
            public ODBFINFO_info info23 = new ODBFINFO_info();
            public ODBFINFO_info info24 = new ODBFINFO_info();
            public ODBFINFO_info info25 = new ODBFINFO_info();
            public ODBFINFO_info info26 = new ODBFINFO_info();
            public ODBFINFO_info info27 = new ODBFINFO_info();
            public ODBFINFO_info info28 = new ODBFINFO_info();
            public ODBFINFO_info info29 = new ODBFINFO_info();
            public ODBFINFO_info info30 = new ODBFINFO_info();
            public ODBFINFO_info info31 = new ODBFINFO_info();
            public ODBFINFO_info info32 = new ODBFINFO_info();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBFINFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string slotname = new string(' ', 12);  /* Slot Name */
            public int fromnum;           /* Number of F-ROM SYSTEM data */
            public ODBFINFO1 info = new ODBFINFO1();
        }

        /* cnc_getfrominfo:read F-ROM information on CNC  */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBFINFORM_info
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string sysname = new string(' ', 12);  /* F-ROM SYSTEM data Name */
            public int fromsize;      /* F-ROM Size */
            public int fromattrib;    /* F-ROM data attribute */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBFINFORM1
        {
            public ODBFINFORM_info info1 = new ODBFINFORM_info();
            public ODBFINFORM_info info2 = new ODBFINFORM_info();
            public ODBFINFORM_info info3 = new ODBFINFORM_info();
            public ODBFINFORM_info info4 = new ODBFINFORM_info();
            public ODBFINFORM_info info5 = new ODBFINFORM_info();
            public ODBFINFORM_info info6 = new ODBFINFORM_info();
            public ODBFINFORM_info info7 = new ODBFINFORM_info();
            public ODBFINFORM_info info8 = new ODBFINFORM_info();
            public ODBFINFORM_info info9 = new ODBFINFORM_info();
            public ODBFINFORM_info info10 = new ODBFINFORM_info();
            public ODBFINFORM_info info11 = new ODBFINFORM_info();
            public ODBFINFORM_info info12 = new ODBFINFORM_info();
            public ODBFINFORM_info info13 = new ODBFINFORM_info();
            public ODBFINFORM_info info14 = new ODBFINFORM_info();
            public ODBFINFORM_info info15 = new ODBFINFORM_info();
            public ODBFINFORM_info info16 = new ODBFINFORM_info();
            public ODBFINFORM_info info17 = new ODBFINFORM_info();
            public ODBFINFORM_info info18 = new ODBFINFORM_info();
            public ODBFINFORM_info info19 = new ODBFINFORM_info();
            public ODBFINFORM_info info20 = new ODBFINFORM_info();
            public ODBFINFORM_info info21 = new ODBFINFORM_info();
            public ODBFINFORM_info info22 = new ODBFINFORM_info();
            public ODBFINFORM_info info23 = new ODBFINFORM_info();
            public ODBFINFORM_info info24 = new ODBFINFORM_info();
            public ODBFINFORM_info info25 = new ODBFINFORM_info();
            public ODBFINFORM_info info26 = new ODBFINFORM_info();
            public ODBFINFORM_info info27 = new ODBFINFORM_info();
            public ODBFINFORM_info info28 = new ODBFINFORM_info();
            public ODBFINFORM_info info29 = new ODBFINFORM_info();
            public ODBFINFORM_info info30 = new ODBFINFORM_info();
            public ODBFINFORM_info info31 = new ODBFINFORM_info();
            public ODBFINFORM_info info32 = new ODBFINFORM_info();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBFINFORM
        {
            public int slotno;            /* Slot Number */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string slotname = new string(' ', 12);  /* Slot Name */
            public int fromnum;           /* Number of F-ROM SYSTEM data */
            public ODBFINFORM1 info = new ODBFINFORM1();
        }

        /* cnc_rdsraminfo:read S-RAM information on CNC */
        /* cnc_getsraminfo:read S-RAM information on CNC */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBSINFO_info
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string sramname = new string(' ', 12); /* S-RAM data Name */
            public int sramsize;     /* S-RAM data Size */
            public short divnumber;    /* Division number of S-RAM file */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname1 = new string(' ', 16);    /* S-RAM data Name1 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname2 = new string(' ', 16);    /* S-RAM data Name2 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname3 = new string(' ', 16);    /* S-RAM data Name3 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname4 = new string(' ', 16);    /* S-RAM data Name4 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname5 = new string(' ', 16);    /* S-RAM data Name5 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string fname6 = new string(' ', 16);    /* S-RAM data Name6 */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSINFO1
        {
            public ODBSINFO_info info1 = new ODBSINFO_info();
            public ODBSINFO_info info2 = new ODBSINFO_info();
            public ODBSINFO_info info3 = new ODBSINFO_info();
            public ODBSINFO_info info4 = new ODBSINFO_info();
            public ODBSINFO_info info5 = new ODBSINFO_info();
            public ODBSINFO_info info6 = new ODBSINFO_info();
            public ODBSINFO_info info7 = new ODBSINFO_info();
            public ODBSINFO_info info8 = new ODBSINFO_info();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSINFO
        {
            public int sramnum;          /* Number of S-RAM data */
            public ODBSINFO1 info = new ODBSINFO1();
        }

        /* cnc_rdsramaddr:read S-RAM address on CNC */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class SRAMADDR
        {
            public short type;          /* SRAM data type */
            public int size;          /* SRAM data size */
            public int offset;        /* offset from top address of SRAM */
        }

        /* cnc_dtsvrdpgdir:read file directory in Data Server */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBDSDIR_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string file_name = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string comment = new string(' ', 64);
            public int size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string date = new string(' ', 16);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDSDIR1
        {
            public ODBDSDIR_data data1 = new ODBDSDIR_data();
            public ODBDSDIR_data data2 = new ODBDSDIR_data();
            public ODBDSDIR_data data3 = new ODBDSDIR_data();
            public ODBDSDIR_data data4 = new ODBDSDIR_data();
            public ODBDSDIR_data data5 = new ODBDSDIR_data();
            public ODBDSDIR_data data6 = new ODBDSDIR_data();
            public ODBDSDIR_data data7 = new ODBDSDIR_data();
            public ODBDSDIR_data data8 = new ODBDSDIR_data();
            public ODBDSDIR_data data9 = new ODBDSDIR_data();
            public ODBDSDIR_data data10 = new ODBDSDIR_data();
            public ODBDSDIR_data data11 = new ODBDSDIR_data();
            public ODBDSDIR_data data12 = new ODBDSDIR_data();
            public ODBDSDIR_data data13 = new ODBDSDIR_data();
            public ODBDSDIR_data data14 = new ODBDSDIR_data();
            public ODBDSDIR_data data15 = new ODBDSDIR_data();
            public ODBDSDIR_data data16 = new ODBDSDIR_data();
            public ODBDSDIR_data data17 = new ODBDSDIR_data();
            public ODBDSDIR_data data18 = new ODBDSDIR_data();
            public ODBDSDIR_data data19 = new ODBDSDIR_data();
            public ODBDSDIR_data data20 = new ODBDSDIR_data();
            public ODBDSDIR_data data21 = new ODBDSDIR_data();
            public ODBDSDIR_data data22 = new ODBDSDIR_data();
            public ODBDSDIR_data data23 = new ODBDSDIR_data();
            public ODBDSDIR_data data24 = new ODBDSDIR_data();
            public ODBDSDIR_data data25 = new ODBDSDIR_data();
            public ODBDSDIR_data data26 = new ODBDSDIR_data();
            public ODBDSDIR_data data27 = new ODBDSDIR_data();
            public ODBDSDIR_data data28 = new ODBDSDIR_data();
            public ODBDSDIR_data data29 = new ODBDSDIR_data();
            public ODBDSDIR_data data30 = new ODBDSDIR_data();
            public ODBDSDIR_data data31 = new ODBDSDIR_data();
            public ODBDSDIR_data data32 = new ODBDSDIR_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDSDIR
        {
            public int file_num;
            public int remainder;
            public short data_num;
            public ODBDSDIR1 data = new ODBDSDIR1();
        }

        /* cnc_dtsvrdset:read setting data for Data Server */
        /* cnc_dtsvwrset:write setting data for Data Server */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBDSSET
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string host_ip = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string host_uname = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string host_passwd = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string host_dir = new string(' ', 128);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string dtsv_mac = new string(' ', 13);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string dtsv_ip = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string dtsv_mask = new string(' ', 16);
        }

        /* cnc_dtsvmntinfo:read maintenance information for Data Server */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDSMNT
        {
            public int empty_cnt;
            public int total_size;
            public int read_ptr;
            public int write_ptr;
        }

        /* cnc_rdposerrs2:read the position deviation S1 and S2 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPSER
        {
            public int poserr1;
            public int poserr2;
        }

        /* cnc_rdctrldi:read the control input signal */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDI_data
        {
            public byte sgnl1;
            public byte sgnl2;
            public byte sgnl3;
            public byte sgnl4;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDI
        {
            public ODBSPDI_data di1 = new ODBSPDI_data();
            public ODBSPDI_data di2 = new ODBSPDI_data();
            public ODBSPDI_data di3 = new ODBSPDI_data();
            public ODBSPDI_data di4 = new ODBSPDI_data();
        }

        /* cnc_rdctrldo:read the control output signal */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDO_data
        {
            public byte sgnl1;
            public byte sgnl2;
            public byte sgnl3;
            public byte sgnl4;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDO
        {
            public ODBSPDO_data do1 = new ODBSPDO_data();
            public ODBSPDO_data do2 = new ODBSPDO_data();
            public ODBSPDO_data do3 = new ODBSPDO_data();
            public ODBSPDO_data do4 = new ODBSPDO_data();
        }

        /* cnc_rdwaveprm:read the parameter of wave diagnosis */
        /* cnc_wrwaveprm:write the parameter of wave diagnosis */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWAVE_io
        {
            public byte adr;
            public byte bit;
            public short no;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWAVE_axis
        {
            public short axis;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class IODBWAVE_u
        {
            [FieldOffset(0)]
            public IODBWAVE_io io = new IODBWAVE_io();
            [FieldOffset(0)]
            public IODBWAVE_axis axis = new IODBWAVE_axis();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWAVE_ch_data
        {
            public short kind;
            public IODBWAVE_u u = new IODBWAVE_u();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWAVE_ch
        {
            public IODBWAVE_ch_data ch1 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch2 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch3 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch4 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch5 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch6 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch7 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch8 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch9 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch10 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch11 = new IODBWAVE_ch_data();
            public IODBWAVE_ch_data ch12 = new IODBWAVE_ch_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWAVE
        {
            public short condition;
            public char trg_adr;
            public byte trg_bit;
            public short trg_no;
            public short delay;
            public short t_range;
            public IODBWAVE_ch ch = new IODBWAVE_ch();
        }

        /* cnc_rdwaveprm2:read the parameter of wave diagnosis 2 */
        /* cnc_wrwaveprm2:write the parameter of wave diagnosis 2 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWVPRM_io
        {
            public byte adr;
            public byte bit;
            public short no;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWVPRM_axis
        {
            public short axis;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class IODBWVPRM_u
        {
            [FieldOffset(0)]
            public IODBWVPRM_io io = new IODBWVPRM_io();
            [FieldOffset(0)]
            public IODBWVPRM_axis axis = new IODBWVPRM_axis();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWVPRM_ch_data
        {
            public short kind;
            public IODBWVPRM_u u = new IODBWVPRM_u();
            public int reserve2;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWVPRM_ch
        {
            public IODBWVPRM_ch_data ch1 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch2 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch3 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch4 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch5 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch6 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch7 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch8 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch9 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch10 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch11 = new IODBWVPRM_ch_data();
            public IODBWVPRM_ch_data ch12 = new IODBWVPRM_ch_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWVPRM
        {
            public short condition;
            public byte trg_adr;
            public byte trg_bit;
            public short trg_no;
            public short reserve1;
            public int delay;
            public int t_range;
            public IODBWVPRM_ch ch = new IODBWVPRM_ch();
        }

        /* cnc_rdwavedata:read the data of wave diagnosis */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBWVDT_io
        {
            public byte adr;
            public byte bit;
            public short no;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBWVDT_axis
        {
            public short axis;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class ODBWVDT_u
        {
            [FieldOffset(0)]
            public ODBWVDT_io io = new ODBWVDT_io();
            [FieldOffset(0)]
            public ODBWVDT_axis axis = new ODBWVDT_axis();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBWVDT
        {
            public short channel;
            public short kind;
            public ODBWVDT_u u = new ODBWVDT_u();
            public byte year;
            public byte month;
            public byte day;
            public byte hour;
            public byte minute;
            public byte second;
            public short t_cycle;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8192)]
            public short[] data;
        }

        /* cnc_rdrmtwaveprm:read the parameter of wave diagnosis for remort diagnosis */
        /* cnc_wrrmtwaveprm:write the parameter of wave diagnosis for remort diagnosis */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBRMTPRM_alm
        {
            public short no;
            public sbyte axis;
            public byte type;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBRMTPRM_io
        {
            public char adr;
            public byte bit;
            public short no;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class IODBRMTPRM_trg
        {
            [FieldOffset(0)]
            public IODBRMTPRM_alm alm = new IODBRMTPRM_alm();
            [FieldOffset(0)]
            public IODBRMTPRM_io io = new IODBRMTPRM_io();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBRMTPRM_smpl
        {
            public char adr;
            public byte bit;
            public short no;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBRMTPRM1
        {
            public IODBRMTPRM_smpl ampl1 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl2 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl3 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl4 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl5 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl6 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl7 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl8 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl9 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl10 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl11 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl12 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl13 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl14 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl15 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl16 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl17 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl18 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl19 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl20 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl21 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl22 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl23 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl24 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl25 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl26 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl27 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl28 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl29 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl30 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl31 = new IODBRMTPRM_smpl();
            public IODBRMTPRM_smpl ampl32 = new IODBRMTPRM_smpl();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBRMTPRM
        {
            public short condition;
            public short reserve;
            public IODBRMTPRM_trg trg = new IODBRMTPRM_trg();
            public int delay;
            public short wv_intrvl;
            public short io_intrvl;
            public short kind1;
            public short kind2;
            public IODBRMTPRM1 ampl = new IODBRMTPRM1();
        }

        /* cnc_rdrmtwavedt:read the data of wave diagnosis for remort diagnosis */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBRMTDT
        {
            public short channel;
            public short kind;
            public byte year;
            public byte month;
            public byte day;
            public byte hour;
            public byte minute;
            public byte second;
            public short t_intrvl;
            public short trg_data;
            public int ins_ptr;
            public short t_delta;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1917)]
            public short[] data;
        }

        /* cnc_rdsavsigadr:read of address for PMC signal batch save */
        /* cnc_wrsavsigadr:write of address for PMC signal batch save */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSIGAD
        {
            public byte adr;
            public byte reserve;
            public short no;
            public short size;
        }

        /* cnc_rdmgrpdata:read M-code group data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBMGRP_data
        {
            public int m_code;
            public short grp_no;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
            public string m_name = new string(' ', 21);
            public byte dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMGRP
        {
            public ODBMGRP_data mgrp1 = new ODBMGRP_data();
            public ODBMGRP_data mgrp2 = new ODBMGRP_data();
            public ODBMGRP_data mgrp3 = new ODBMGRP_data();
            public ODBMGRP_data mgrp4 = new ODBMGRP_data();
            public ODBMGRP_data mgrp5 = new ODBMGRP_data();
            public ODBMGRP_data mgrp6 = new ODBMGRP_data();
            public ODBMGRP_data mgrp7 = new ODBMGRP_data();
            public ODBMGRP_data mgrp8 = new ODBMGRP_data();
            public ODBMGRP_data mgrp9 = new ODBMGRP_data();
            public ODBMGRP_data mgrp10 = new ODBMGRP_data();
        }

        /* cnc_wrmgrpdata:write M-code group data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBMGRP
        {
            public short s_no;
            public short dummy;
            public short num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 500)]
            public short[] group = new short[500];
        }

        /* cnc_rdexecmcode:read executing M-code group data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXEM_data
        {
            public int no;
            public short flag;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXEM1
        {
            public ODBEXEM_data m_code1 = new ODBEXEM_data();
            public ODBEXEM_data m_code2 = new ODBEXEM_data();
            public ODBEXEM_data m_code3 = new ODBEXEM_data();
            public ODBEXEM_data m_code4 = new ODBEXEM_data();
            public ODBEXEM_data m_code5 = new ODBEXEM_data();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBEXEM
        {
            public short grp_no;
            public short mem_no;
            public ODBEXEM1 m_code = new ODBEXEM1();
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
            public string m_name = new string(' ', 21);
            public byte dummy;
        }

        /* cnc_rdrstrmcode:read program restart M-code group data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class M_CODE_data
        {
            public int no;
            public short flag;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class M_CODE1
        {
            public M_CODE_data m_code1 = new M_CODE_data();
            public M_CODE_data m_code2 = new M_CODE_data();
            public M_CODE_data m_code3 = new M_CODE_data();
            public M_CODE_data m_code4 = new M_CODE_data();
            public M_CODE_data m_code5 = new M_CODE_data();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBRSTRM
        {
            public short grp_no;
            public short mem_no;
            public M_CODE1 m_code = new M_CODE1();
        }

        /* cnc_rdproctime:read processing time stamp data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPTIME_data
        {
            public int prg_no;
            public short hour;
            public byte minute;
            public byte second;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPTIME1
        {
            public ODBPTIME_data data1 = new ODBPTIME_data();
            public ODBPTIME_data data2 = new ODBPTIME_data();
            public ODBPTIME_data data3 = new ODBPTIME_data();
            public ODBPTIME_data data4 = new ODBPTIME_data();
            public ODBPTIME_data data5 = new ODBPTIME_data();
            public ODBPTIME_data data6 = new ODBPTIME_data();
            public ODBPTIME_data data7 = new ODBPTIME_data();
            public ODBPTIME_data data8 = new ODBPTIME_data();
            public ODBPTIME_data data9 = new ODBPTIME_data();
            public ODBPTIME_data data10 = new ODBPTIME_data();
        } /* In case that the number of data is 10 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPTIME
        {
            public short num;
            public ODBPTIME1 data = new ODBPTIME1();
        }

        /* cnc_rdprgdirtime:read program directory for processing time data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIRTM_data
        {
            public int prg_no;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
            public string comment = new string(' ', 51);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string cuttime = new string(' ', 13);
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIRTM
        {
            public PRGDIRTM_data data1 = new PRGDIRTM_data();
            public PRGDIRTM_data data2 = new PRGDIRTM_data();
            public PRGDIRTM_data data3 = new PRGDIRTM_data();
            public PRGDIRTM_data data4 = new PRGDIRTM_data();
            public PRGDIRTM_data data5 = new PRGDIRTM_data();
            public PRGDIRTM_data data6 = new PRGDIRTM_data();
            public PRGDIRTM_data data7 = new PRGDIRTM_data();
            public PRGDIRTM_data data8 = new PRGDIRTM_data();
            public PRGDIRTM_data data9 = new PRGDIRTM_data();
            public PRGDIRTM_data data10 = new PRGDIRTM_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdprogdir2:read program directory 2 */
#if (!ONO8D)
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR2_data
        {
            public short number;
            public int length;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
            public string comment = new string(' ', 51);
            public byte dummy;
        }
#else
    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi,Pack=4)]
    public class PRGDIR2_data
    {
        public int     number ;
        public int     length ;
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=51)]
        public string  comment= new string(' ',51) ;
        public byte    dummy ;
    }
#endif
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR2
        {
            public PRGDIR2_data dir1 = new PRGDIR2_data();
            public PRGDIR2_data dir2 = new PRGDIR2_data();
            public PRGDIR2_data dir3 = new PRGDIR2_data();
            public PRGDIR2_data dir4 = new PRGDIR2_data();
            public PRGDIR2_data dir5 = new PRGDIR2_data();
            public PRGDIR2_data dir6 = new PRGDIR2_data();
            public PRGDIR2_data dir7 = new PRGDIR2_data();
            public PRGDIR2_data dir8 = new PRGDIR2_data();
            public PRGDIR2_data dir9 = new PRGDIR2_data();
            public PRGDIR2_data dir10 = new PRGDIR2_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdprogdir3:read program directory 3 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class DIR3_MDATE
        {
            public short year;
            public short month;
            public short day;
            public short hour;
            public short minute;
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class DIR3_CDATE
        {
            public short year;
            public short month;
            public short day;
            public short hour;
            public short minute;
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR3_data
        {
            public int number;
            public int length;
            public int page;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 52)]
            public string comment = new string(' ', 52);
            public DIR3_MDATE mdate = new DIR3_MDATE();
            public DIR3_CDATE cdate = new DIR3_CDATE();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR3
        {
            public PRGDIR3_data dir1 = new PRGDIR3_data();
            public PRGDIR3_data dir2 = new PRGDIR3_data();
            public PRGDIR3_data dir3 = new PRGDIR3_data();
            public PRGDIR3_data dir4 = new PRGDIR3_data();
            public PRGDIR3_data dir5 = new PRGDIR3_data();
            public PRGDIR3_data dir6 = new PRGDIR3_data();
            public PRGDIR3_data dir7 = new PRGDIR3_data();
            public PRGDIR3_data dir8 = new PRGDIR3_data();
            public PRGDIR3_data dir9 = new PRGDIR3_data();
            public PRGDIR3_data dir10 = new PRGDIR3_data();
        } /* In case that the number of data is 10 */


        /* cnc_rdprogdir4:read program directory 4 */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class DIR4_MDATE
        {
            public short year;
            public short month;
            public short day;
            public short hour;
            public short minute;
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class DIR4_CDATE
        {
            public short year;
            public short month;
            public short day;
            public short hour;
            public short minute;
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR4_data
        {
            public int number;
            public int length;
            public int page;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 52)]
            public string comment = new string(' ', 52);
            public DIR4_MDATE mdate = new DIR4_MDATE();
            public DIR4_CDATE cdate = new DIR4_CDATE();
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class PRGDIR4
        {
            public PRGDIR4_data dir1 = new PRGDIR4_data();
            public PRGDIR4_data dir2 = new PRGDIR4_data();
            public PRGDIR4_data dir3 = new PRGDIR4_data();
            public PRGDIR4_data dir4 = new PRGDIR4_data();
            public PRGDIR4_data dir5 = new PRGDIR4_data();
            public PRGDIR4_data dir6 = new PRGDIR4_data();
            public PRGDIR4_data dir7 = new PRGDIR4_data();
            public PRGDIR4_data dir8 = new PRGDIR4_data();
            public PRGDIR4_data dir9 = new PRGDIR4_data();
            public PRGDIR4_data dir10 = new PRGDIR4_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdcomparam:read communication parameter for DNC1, DNC2, OSI-Ethernet */
        /* cnc_wrcomparam:write communication parameter for DNC1, DNC2, OSI-Ethernet */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBCPRM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string NcApli = new string(' ', 65);
            public byte Dummy1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string HostApli = new string(' ', 65);
            public byte Dummy2;
            public uint StatPstv;
            public uint StatNgtv;
            public uint Statmask;
            public uint AlarmStat;
            public uint PsclHaddr;
            public uint PsclLaddr;
            public ushort SvcMode1;
            public ushort SvcMode2;
            public int FileTout;
            public int RemTout;
        }

        /* cnc_rdintchk:read interference check */
        /* cnc_wrintchk:write interference check */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBINT
        {
            public short datano_s;   /* start offset No. */
            public short type;       /* kind of position */
            public short datano_e;   /* end offset No. */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 3)]
            public int[] data = new int[8 * 3];       /* position value of area for not attach */
        }

        /* cnc_rdwkcdshft:read work coordinate shift */
        /* cnc_wrwkcdshft:write work coordinate shift */
        /* cnc_rdwkcdsfms:read work coordinate shift measure */
        /* cnc_wrwkcdsfms:write work coordinate shift measure */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBWCSF
        {
            public short datano;    /* datano           */
            public short type;      /* axis number      */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] data = new int[MAX_AXIS];       /* data             */
        }

        /* cnc_rdomhisinfo:read operator message history information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOMIF
        {
            public ushort om_max;   /* maximum operator message history */
            public ushort om_sum;   /* actually operator message history */
            public ushort om_char;  /* maximum character (include NULL) */
        }

        /* cnc_rdomhistry:read operator message history */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBOMHIS_data
        {
            public short om_no;  /* operator message number */
            public short year;   /* year */
            public short month;  /* month */
            public short day;    /* day */
            public short hour;   /* hour */
            public short minute; /* mimute */
            public short second; /* second */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string om_msg = new string(' ', 256);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBOMHIS
        {
            public ODBOMHIS_data omhis1 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis2 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis3 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis4 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis5 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis6 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis7 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis8 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis9 = new ODBOMHIS_data();
            public ODBOMHIS_data omhis10 = new ODBOMHIS_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdbtofsr:read b-axis tool offset value(area specified) */
        /* cnc_wrbtofsr:write b-axis tool offset value(area specified) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBBTO
        {
            public short datano_s;     /* start offset number */
            public short type;     /* offset type */
            public short datano_e;     /* end offset number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public int[] ofs = new int[18];        /* offset */
        } /* In case that the number of data is 9 (B type) */

        /* cnc_rdbtofsinfo:read b-axis tool offset information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBBTLINF
        {
            public short ofs_type;   /* memory type */
            public short use_no;     /* sum of b-axis offset */
            public short sub_no;     /* sub function number of offset cancel */
        }

        /* cnc_rdbaxis:read b-axis command */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBBAXIS
        {
            public short flag;        /* b-axis command exist or not */
            public short command;     /* b-axis command */
            public ushort speed;       /* b-axis speed */
            public int sub_data;    /* b-axis sub data */
        }

        /* cnc_rdsyssoft:read CNC system soft series and version */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBSYSS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_p;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_l;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] module_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] soft_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series16 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version16 = new string(' ', 5);
            public short soft_inst;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string boot_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string boot_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string servo_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string servo_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string ladder_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string ladder_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrlib_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrlib_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrapl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrapl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl1_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl1_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl2_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl2_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl3_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl3_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exelib_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exelib_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exeapl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exeapl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string int_vga_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string int_vga_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string out_vga_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string out_vga_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmm_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmm_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_mng_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_mng_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shin_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shin_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shout_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shout_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_c_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_c_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_edit_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_edit_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_mng_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_mng_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_apl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_apl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl4_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl4_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr2_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr2_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr3_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr3_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string eth_boot_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string eth_boot_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 5)]
            public byte[] reserve;
        }

        /* cnc_rdsyssoft2:read CNC system soft series and version (2) */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBSYSS2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_p;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] slot_no_l;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] module_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] soft_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_series16 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version1 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version2 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version3 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version4 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version5 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version6 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version7 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version8 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version9 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version10 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version11 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version12 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version13 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version14 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version15 = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string soft_version16 = new string(' ', 5);
            public short soft_inst;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string boot_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string boot_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string servo_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string servo_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string ladder_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string ladder_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrlib_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrlib_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrapl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcrapl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl1_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl1_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl2_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl2_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl3_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl3_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exelib_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exelib_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exeapl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string c_exeapl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string int_vga_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string int_vga_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string out_vga_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string out_vga_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmm_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmm_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_mng_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_mng_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shin_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shin_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shout_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_shout_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_c_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_c_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_edit_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string pmc_edit_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_mng_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_mng_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_apl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string lddr_apl_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl4_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string spl4_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr2_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr2_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr3_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string mcr3_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string eth_boot_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string eth_boot_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 5)]
            public byte[] reserve;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string embEthe_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string embEthe_ver = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38 * 5)]
            public byte[] reserve2;
        }

        /* cnc_rdsyssoft3:read CNC system soft series and version (3) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSS3_data
        {
            public short soft_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public char[] soft_series;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public char[] soft_edition;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSS3
        {
            public ODBSYSS3_data p1 = new ODBSYSS3_data();
            public ODBSYSS3_data p2 = new ODBSYSS3_data();
            public ODBSYSS3_data p3 = new ODBSYSS3_data();
            public ODBSYSS3_data p4 = new ODBSYSS3_data();
            public ODBSYSS3_data p5 = new ODBSYSS3_data();
            public ODBSYSS3_data p6 = new ODBSYSS3_data();
            public ODBSYSS3_data p7 = new ODBSYSS3_data();
            public ODBSYSS3_data p8 = new ODBSYSS3_data();
            public ODBSYSS3_data p9 = new ODBSYSS3_data();
            public ODBSYSS3_data p10 = new ODBSYSS3_data();
            public ODBSYSS3_data p11 = new ODBSYSS3_data();
            public ODBSYSS3_data p12 = new ODBSYSS3_data();
            public ODBSYSS3_data p13 = new ODBSYSS3_data();
            public ODBSYSS3_data p14 = new ODBSYSS3_data();
            public ODBSYSS3_data p15 = new ODBSYSS3_data();
            public ODBSYSS3_data p16 = new ODBSYSS3_data();
            public ODBSYSS3_data p17 = new ODBSYSS3_data();
            public ODBSYSS3_data p18 = new ODBSYSS3_data();
            public ODBSYSS3_data p19 = new ODBSYSS3_data();
            public ODBSYSS3_data p20 = new ODBSYSS3_data();
            public ODBSYSS3_data p21 = new ODBSYSS3_data();
            public ODBSYSS3_data p22 = new ODBSYSS3_data();
            public ODBSYSS3_data p23 = new ODBSYSS3_data();
            public ODBSYSS3_data p24 = new ODBSYSS3_data();
            public ODBSYSS3_data p25 = new ODBSYSS3_data();
            public ODBSYSS3_data p26 = new ODBSYSS3_data();
            public ODBSYSS3_data p27 = new ODBSYSS3_data();
            public ODBSYSS3_data p28 = new ODBSYSS3_data();
            public ODBSYSS3_data p29 = new ODBSYSS3_data();
            public ODBSYSS3_data p30 = new ODBSYSS3_data();
            public ODBSYSS3_data p31 = new ODBSYSS3_data();
            public ODBSYSS3_data p32 = new ODBSYSS3_data();
            public ODBSYSS3_data p33 = new ODBSYSS3_data();
            public ODBSYSS3_data p34 = new ODBSYSS3_data();
            public ODBSYSS3_data p35 = new ODBSYSS3_data();
            public ODBSYSS3_data p36 = new ODBSYSS3_data();
            public ODBSYSS3_data p37 = new ODBSYSS3_data();
            public ODBSYSS3_data p38 = new ODBSYSS3_data();
            public ODBSYSS3_data p39 = new ODBSYSS3_data();
            public ODBSYSS3_data p40 = new ODBSYSS3_data();
        }

        /* cnc_rdsyshard:read CNC system hard info */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSH_data
        {
            public uint id1;
            public uint id2;
            public short group_id;
            public short hard_id;
            public short hard_num;
            public short slot_no;
            public short id1_format;
            public short id2_format;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSH
        {
            public ODBSYSH_data data1 = new ODBSYSH_data();
            public ODBSYSH_data data2 = new ODBSYSH_data();
            public ODBSYSH_data data3 = new ODBSYSH_data();
            public ODBSYSH_data data4 = new ODBSYSH_data();
            public ODBSYSH_data data5 = new ODBSYSH_data();
            public ODBSYSH_data data6 = new ODBSYSH_data();
            public ODBSYSH_data data7 = new ODBSYSH_data();
            public ODBSYSH_data data8 = new ODBSYSH_data();
            public ODBSYSH_data data9 = new ODBSYSH_data();
            public ODBSYSH_data data10 = new ODBSYSH_data();
            public ODBSYSH_data data11 = new ODBSYSH_data();
            public ODBSYSH_data data12 = new ODBSYSH_data();
            public ODBSYSH_data data13 = new ODBSYSH_data();
            public ODBSYSH_data data14 = new ODBSYSH_data();
            public ODBSYSH_data data15 = new ODBSYSH_data();
            public ODBSYSH_data data16 = new ODBSYSH_data();
            public ODBSYSH_data data17 = new ODBSYSH_data();
            public ODBSYSH_data data18 = new ODBSYSH_data();
            public ODBSYSH_data data19 = new ODBSYSH_data();
            public ODBSYSH_data data20 = new ODBSYSH_data();
            public ODBSYSH_data data21 = new ODBSYSH_data();
            public ODBSYSH_data data22 = new ODBSYSH_data();
            public ODBSYSH_data data23 = new ODBSYSH_data();
            public ODBSYSH_data data24 = new ODBSYSH_data();
            public ODBSYSH_data data25 = new ODBSYSH_data();
        }

        /* cnc_rdmdlconfig:read CNC module configuration information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMDLC
        {
            public short from;
            public short dram;
            public short sram;
            public short pmc;
            public short crtc;
            public short servo12;
            public short servo34;
            public short servo56;
            public short servo78;
            public short sic;
            public short pos_lsi;
            public short hi_aio;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public short[] reserve;
            public short drmmrc;
            public short drmarc;
            public short pmcmrc;
            public short dmaarc;
            public short iopt;
            public short hdiio;
            public short gm2gr1;
            public short crtgr2;
            public short gm1gr2;
            public short gm2gr2;
            public short cmmrb;
            public short sv5axs;
            public short sv7axs;
            public short sicaxs;
            public short posaxs;
            public short hamaxs;
            public short romr64;
            public short srmr64;
            public short dr1r64;
            public short dr2r64;
            public short iopio2;
            public short hdiio2;
            public short cmmrb2;
            public short romfap;
            public short srmfap;
            public short drmfap;
            public short drmare;
            public short pmcmre;
            public short dmaare;
            public short frmbgg;
            public short drmbgg;
            public short asrbgg;
            public short edtpsc;
            public short slcpsc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public short[] reserve2;
        }

        /* cnc_rdpscdproc:read processing condition file (processing data) */
        /* cnc_wrpscdproc:write processing condition file (processing data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSCD_data
        {
            public short slct;
            public int feed;
            public short power;
            public short freq;
            public short duty;
            public short g_press;
            public short g_kind;
            public short g_ready_t;
            public short displace;
            public int supple;
            public short edge_slt;
            public short appr_slt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] reserve = new short[5];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPSCD
        {
            public IODBPSCD_data data1 = new IODBPSCD_data();
            public IODBPSCD_data data2 = new IODBPSCD_data();
            public IODBPSCD_data data3 = new IODBPSCD_data();
            public IODBPSCD_data data4 = new IODBPSCD_data();
            public IODBPSCD_data data5 = new IODBPSCD_data();
            public IODBPSCD_data data6 = new IODBPSCD_data();
            public IODBPSCD_data data7 = new IODBPSCD_data();
            public IODBPSCD_data data8 = new IODBPSCD_data();
            public IODBPSCD_data data9 = new IODBPSCD_data();
            public IODBPSCD_data data10 = new IODBPSCD_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdpscdpirc:read processing condition file (piercing data) */
        /* cnc_wrpscdpirc:write processing condition file (piercing data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPIRC_data
        {
            public short slct;
            public short power;
            public short freq;
            public short duty;
            public short i_freq;
            public short i_duty;
            public short step_t;
            public short step_sum;
            public int pier_t;
            public short g_press;
            public short g_kind;
            public short g_time;
            public short def_pos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] reserve = new short[4];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPIRC
        {
            public IODBPIRC_data data1 = new IODBPIRC_data();
            public IODBPIRC_data data2 = new IODBPIRC_data();
            public IODBPIRC_data data3 = new IODBPIRC_data();
        }

        /* cnc_rdpscdedge:read processing condition file (edging data) */
        /* cnc_wrpscdedge:write processing condition file (edging data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBEDGE_data
        {
            public short slct;
            public short angle;
            public short power;
            public short freq;
            public short duty;
            public int pier_t;
            public short g_press;
            public short g_kind;
            public int r_len;
            public short r_feed;
            public short r_freq;
            public short r_duty;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] reserve = new short[5];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBEDGE
        {
            public IODBEDGE_data data1 = new IODBEDGE_data();
            public IODBEDGE_data data2 = new IODBEDGE_data();
            public IODBEDGE_data data3 = new IODBEDGE_data();
            public IODBEDGE_data data4 = new IODBEDGE_data();
            public IODBEDGE_data data5 = new IODBEDGE_data();
        }

        /* cnc_rdpscdslop:read processing condition file (slope data) */
        /* cnc_wrpscdslop:write processing condition file (slope data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLOP_data
        {
            public int slct;
            public int upleng;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public short[] upsp = new short[10];
            public int dwleng;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public short[] dwsp = new short[10];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public short[] reserve = new short[10];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLOP
        {
            public IODBSLOP_data data1 = new IODBSLOP_data();
            public IODBSLOP_data data2 = new IODBSLOP_data();
            public IODBSLOP_data data3 = new IODBSLOP_data();
            public IODBSLOP_data data4 = new IODBSLOP_data();
            public IODBSLOP_data data5 = new IODBSLOP_data();
        }

        /* cnc_rdlpwrdty:read power controll duty data */
        /* cnc_wrlpwrdty:write power controll duty data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBLPWDT
        {
            public short slct;
            public short dty_const;
            public short dty_min;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public short[] reserve = new short[6];
        }

        /* cnc_rdlpwrdat:read laser power data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBLOPDT
        {
            public short slct;
            public short pwr_mon;
            public short pwr_ofs;
            public short pwr_act;
            public int feed_act;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] reserve;
        }

        /* cnc_rdlagslt:read laser assist gas selection */
        /* cnc_wrlagslt:write laser assist gas selection */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBLAGSL
        {
            public short slct;
            public short ag_slt;
            public short agflow_slt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public short[] reserve = new short[6];
        }

        /* cnc_rdlagst:read laser assist gas flow */
        /* cnc_wrlagst:write laser assist gas flow */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class GASFLOW
        {
            public short slct;
            public short pre_time;
            public short pre_press;
            public short proc_press;
            public short end_time;
            public short end_press;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] reserve = new short[3];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBLAGST
        {
            public GASFLOW data1 = new GASFLOW();
            public GASFLOW data2 = new GASFLOW();
            public GASFLOW data3 = new GASFLOW();
        }

        /* cnc_rdledgprc:read laser power for edge processing */
        /* cnc_wrledgprc:write laser power for edge processing */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBLEGPR
        {
            public short slct;
            public short power;
            public short freq;
            public short duty;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] reserve = new short[5];
        }

        /* cnc_rdlprcprc:read laser power for piercing */
        /* cnc_wrlprcprc:write laser power for piercing */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBLPCPR
        {
            public short slct;
            public short power;
            public short freq;
            public short duty;
            public int time;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] reserve = new short[4];
        }

        /* cnc_rdlcmddat:read laser command data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBLCMDT
        {
            public short slct;
            public int feed;
            public short power;
            public short freq;
            public short duty;
            public short g_kind;
            public short g_ready_t;
            public short g_press;
            public short error;
            public int dsplc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public short[] reserve = new short[7];
        }

        /* cnc_rdlactnum:read active number */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBLACTN
        {
            public short slct;
            public short act_proc;
            public short act_pirce;
            public short act_slop;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] reserve = new short[5];
        }

        /* cnc_rdlcmmt:read laser comment */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBLCMMT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string comment = new string(' ', 25);
        }

        /* cnc_rdpwofsthis:read power correction factor history data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPWOFST_data
        {
            public int pwratio;
            public int rfvolt;
            public ushort year;
            public ushort month;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPWOFST
        {
            public ODBPWOFST_data data1 = new ODBPWOFST_data();
            public ODBPWOFST_data data2 = new ODBPWOFST_data();
            public ODBPWOFST_data data3 = new ODBPWOFST_data();
            public ODBPWOFST_data data4 = new ODBPWOFST_data();
            public ODBPWOFST_data data5 = new ODBPWOFST_data();
            public ODBPWOFST_data data6 = new ODBPWOFST_data();
            public ODBPWOFST_data data7 = new ODBPWOFST_data();
            public ODBPWOFST_data data8 = new ODBPWOFST_data();
            public ODBPWOFST_data data9 = new ODBPWOFST_data();
            public ODBPWOFST_data data10 = new ODBPWOFST_data();
            public ODBPWOFST_data data11 = new ODBPWOFST_data();
            public ODBPWOFST_data data12 = new ODBPWOFST_data();
            public ODBPWOFST_data data13 = new ODBPWOFST_data();
            public ODBPWOFST_data data14 = new ODBPWOFST_data();
            public ODBPWOFST_data data15 = new ODBPWOFST_data();
            public ODBPWOFST_data data16 = new ODBPWOFST_data();
            public ODBPWOFST_data data17 = new ODBPWOFST_data();
            public ODBPWOFST_data data18 = new ODBPWOFST_data();
            public ODBPWOFST_data data19 = new ODBPWOFST_data();
            public ODBPWOFST_data data20 = new ODBPWOFST_data();
            public ODBPWOFST_data data21 = new ODBPWOFST_data();
            public ODBPWOFST_data data22 = new ODBPWOFST_data();
            public ODBPWOFST_data data23 = new ODBPWOFST_data();
            public ODBPWOFST_data data24 = new ODBPWOFST_data();
            public ODBPWOFST_data data25 = new ODBPWOFST_data();
            public ODBPWOFST_data data26 = new ODBPWOFST_data();
            public ODBPWOFST_data data27 = new ODBPWOFST_data();
            public ODBPWOFST_data data28 = new ODBPWOFST_data();
            public ODBPWOFST_data data29 = new ODBPWOFST_data();
            public ODBPWOFST_data data30 = new ODBPWOFST_data();
        }

        /* cnc_rdmngtime:read management time */
        /* cnc_wrmngtime:write management time */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMNGTIME_data
        {
            public uint life;
            public uint total;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMNGTIME
        {
            public IODBMNGTIME_data data1 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data2 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data3 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data4 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data5 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data6 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data7 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data8 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data9 = new IODBMNGTIME_data();
            public IODBMNGTIME_data data10 = new IODBMNGTIME_data();
        } /* In case that the number of data is 10 */

        /* cnc_rddischarge:read data related to electrical discharge at power correction ends */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDISCHRG
        {
            public ushort aps;
            public ushort year;
            public ushort month;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public short hpc;
            public short hfq;
            public short hdt;
            public short hpa;
            public int hce;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] rfi;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] rfv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dci;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dcv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dcw;
        }

        /* cnc_rddischrgalm:read alarm history data related to electrical discharg */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDISCHRGALM_data
        {
            public ushort year;
            public ushort month;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public int almnum;
            public uint psec;
            public short hpc;
            public short hfq;
            public short hdt;
            public short hpa;
            public int hce;
            public ushort asq;
            public ushort psu;
            public ushort aps;
            public short dummy;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] rfi;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] rfv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dci;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dcv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] dcw;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] almcd;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBDISCHRGALM
        {
            public ODBDISCHRGALM_data data1 = new ODBDISCHRGALM_data();
            public ODBDISCHRGALM_data data2 = new ODBDISCHRGALM_data();
            public ODBDISCHRGALM_data data3 = new ODBDISCHRGALM_data();
            public ODBDISCHRGALM_data data4 = new ODBDISCHRGALM_data();
            public ODBDISCHRGALM_data data5 = new ODBDISCHRGALM_data();
        }

        /* cnc_gettimer:get date and time from cnc */
        /* cnc_settimer:set date and time for cnc */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TIMER_DATE
        {
            public short year;
            public short month;
            public short date;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TIMER_TIME
        {
            public short hour;
            public short minute;
            public short second;
        }
        [StructLayout(LayoutKind.Explicit)]
        public class IODBTIMER
        {
            [FieldOffset(0)]
            public short type;
            [FieldOffset(2)]
            public short dummy;
            [FieldOffset(4)]
            public TIMER_DATE date;
            [FieldOffset(4)]
            public TIMER_TIME time;
        }

        /* cnc_rdtimer:read timer data from cnc */
        /* cnc_wrtimer:write timer data for cnc */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTIME
        {
            public int minute;
            public int msec;
        }

        /* cnc_rdtlctldata: read tool controll data */
        /* cnc_wrtlctldata: write tool controll data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLCTL
        {
            public short slct;
            public short used_tool;
            public short turret_indx;
            public int zero_tl_no;
            public int t_axis_move;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] total_punch = new int[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public short[] reserve = new short[11];
        }

        /* cnc_rdtooldata: read tool data */
        /* cnc_wrtooldata: read tool data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLDT_data
        {
            public short slct;
            public int tool_no;
            public int x_axis_ofs;
            public int y_axis_ofs;
            public int turret_pos;
            public int chg_tl_no;
            public int punch_count;
            public int tool_life;
            public int m_tl_radius;
            public int m_tl_angle;
            public byte tl_shape;
            public int tl_size_i;
            public int tl_size_j;
            public int tl_angle;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] reserve = new int[3];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLDT
        {
            public IODBTLDT_data data1 = new IODBTLDT_data();
            public IODBTLDT_data data2 = new IODBTLDT_data();
            public IODBTLDT_data data3 = new IODBTLDT_data();
            public IODBTLDT_data data4 = new IODBTLDT_data();
            public IODBTLDT_data data5 = new IODBTLDT_data();
            public IODBTLDT_data data6 = new IODBTLDT_data();
            public IODBTLDT_data data7 = new IODBTLDT_data();
            public IODBTLDT_data data8 = new IODBTLDT_data();
            public IODBTLDT_data data9 = new IODBTLDT_data();
            public IODBTLDT_data data10 = new IODBTLDT_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdmultitldt: read multi tool data */
        /* cnc_wrmultitldt: write multi tool data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMLTTL_data
        {
            public short slct;
            public short m_tl_no;
            public int m_tl_radius;
            public int m_tl_angle;
            public int x_axis_ofs;
            public int y_axis_ofs;
            public byte tl_shape;
            public int tl_size_i;
            public int tl_size_j;
            public int tl_angle;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public int[] reserve = new int[7];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMLTTL
        {
            public IODBMLTTL_data data1 = new IODBMLTTL_data();
            public IODBMLTTL_data data2 = new IODBMLTTL_data();
            public IODBMLTTL_data data3 = new IODBMLTTL_data();
            public IODBMLTTL_data data4 = new IODBMLTTL_data();
            public IODBMLTTL_data data5 = new IODBMLTTL_data();
            public IODBMLTTL_data data6 = new IODBMLTTL_data();
            public IODBMLTTL_data data7 = new IODBMLTTL_data();
            public IODBMLTTL_data data8 = new IODBMLTTL_data();
            public IODBMLTTL_data data9 = new IODBMLTTL_data();
            public IODBMLTTL_data data10 = new IODBMLTTL_data();
        } /* In case that the number of data is 10 */

        /* cnc_rdmtapdata: read multi tap data */
        /* cnc_wrmtapdata: write multi tap data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMTAP_data
        {
            public short slct;
            public int tool_no;
            public int x_axis_ofs;
            public int y_axis_ofs;
            public int punch_count;
            public int tool_life;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public int[] reserve = new int[11];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBMTAP
        {
            public IODBMTAP_data data1 = new IODBMTAP_data();
            public IODBMTAP_data data2 = new IODBMTAP_data();
            public IODBMTAP_data data3 = new IODBMTAP_data();
            public IODBMTAP_data data4 = new IODBMTAP_data();
            public IODBMTAP_data data5 = new IODBMTAP_data();
            public IODBMTAP_data data6 = new IODBMTAP_data();
            public IODBMTAP_data data7 = new IODBMTAP_data();
            public IODBMTAP_data data8 = new IODBMTAP_data();
            public IODBMTAP_data data9 = new IODBMTAP_data();
            public IODBMTAP_data data10 = new IODBMTAP_data();
        }

        /* cnc_rdtoolinfo: read tool information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPTLINF
        {
            public short tld_max;
            public short mlt_max;
            public short reserve;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] tld_size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] mlt_size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] reserves;
        }

        /* cnc_rdsafetyzone: read safetyzone data */
        /* cnc_wrsafetyzone: write safetyzone data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSAFE_data
        {
            public short slct;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] data = new int[3];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSAFE
        {
            public IODBSAFE_data data1 = new IODBSAFE_data();
            public IODBSAFE_data data2 = new IODBSAFE_data();
            public IODBSAFE_data data3 = new IODBSAFE_data();
            public IODBSAFE_data data4 = new IODBSAFE_data();
        } /* In case that the number of data is 4 */

        /* cnc_rdtoolzone: read toolzone data */
        /* cnc_wrtoolzone: write toolzone data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLZN_data
        {
            public short slct;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] data = new int[12];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBTLZN
        {
            public IODBTLZN_data data1 = new IODBTLZN_data();
            public IODBTLZN_data data2 = new IODBTLZN_data();
            public IODBTLZN_data data3 = new IODBTLZN_data();
            public IODBTLZN_data data4 = new IODBTLZN_data();
            public IODBTLZN_data data5 = new IODBTLZN_data();
            public IODBTLZN_data data6 = new IODBTLZN_data();
            public IODBTLZN_data data7 = new IODBTLZN_data();
            public IODBTLZN_data data8 = new IODBTLZN_data();
            public IODBTLZN_data data9 = new IODBTLZN_data();
            public IODBTLZN_data data10 = new IODBTLZN_data();
            public IODBTLZN_data data11 = new IODBTLZN_data();
            public IODBTLZN_data data12 = new IODBTLZN_data();
        } /* In case that the number of data is 12 */

        /* cnc_rdacttlzone: read active toolzone data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBACTTLZN
        {
            public short act_no;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] data;
        }

        /* cnc_rdbrstrinfo:read block restart information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBBRS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] dest;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] dist;
        } /*  In case that the number of axes is MAX_AXIS */

        /* cnc_rdradofs:read tool radius offset for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBROFS
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] pln_axes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] ofsvct;
        }

        /* cnc_rdlenofs:read tool length offset for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBLOFS
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ofsvct;
        } /*  In case that the number of axes is MAX_AXIS */

        /* cnc_rdfixcycle:read fixed cycle for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBFIX
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] pln_axes;
            public short drl_axes;
            public int i_pos;
            public int r_pos;
            public int z_pos;
            public int cmd_cnt;
            public int act_cnt;
            public int cut;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] shift;
        }

        /* cnc_rdcdrotate:read coordinate rotate for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBROT
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] pln_axes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] center;
            public int angle;
        }

        /* cnc_rd3dcdcnv:read 3D coordinate convert for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DCD
        {
            public short mode;
            public short dno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] cd_axes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * 3)]
            public int[] center;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * 3)]
            public int[] direct;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] angle;
        }

        /* cnc_rdmirimage:read programable mirror image for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBMIR
        {
            public short mode;
            public int mir_flag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] mir_pos;
        } /*  In case that the number of axes is MAX_AXIS */

        /* cnc_rdscaling:read scaling data for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSCL
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] center;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] magnif;
        } /*  In case that the number of axes is MAX_AXIS */

        /* cnc_rd3dtofs:read 3D tool offset for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DTO
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] ofs_axes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] ofsvct;
        }

        /* cnc_rdposofs:read tool position offset for position data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPOFS
        {
            public short mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public int[] ofsvct;
        } /*  In case that the number of axes is MAX_AXIS */

        /* cnc_rdhpccset:read hpcc setting data */
        /* cnc_wrhpccset:write hpcc setting data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBHPST
        {
            public short slct;
            public short hpcc;
            public short multi;
            public short ovr1;
            public short ign_f;
            public short foward;
            public int max_f;
            public short ovr2;
            public short ovr3;
            public short ovr4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public int[] reserve = new int[7];
        }

        /* cnc_rdhpcctupr:read hpcc tuning data ( parameter input ) */
        /* cnc_wrhpcctupr:write hpcc tuning data ( parameter input ) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBHPPR_tune
        {
            public short slct;
            public short diff;
            public short fine;
            public short acc_lv;
            public int max_f;
            public short bipl;
            public short aipl;
            public int corner;
            public short clamp;
            public int radius;
            public int max_cf;
            public int min_cf;
            public int foward;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] reserve = new int[5];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBHPPR
        {
            public IODBHPPR_tune tune1 = new IODBHPPR_tune();
            public IODBHPPR_tune tune2 = new IODBHPPR_tune();
            public IODBHPPR_tune tune3 = new IODBHPPR_tune();
        }

        /* cnc_rdhpcctuac:read hpcc tuning data ( acc input ) */
        /* cnc_wrhpcctuac:write hpcc tuning data ( acc input ) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBHPAC_tune
        {
            public short slct;
            public short diff;
            public short fine;
            public short acc_lv;
            public int bipl;
            public short aipl;
            public int corner;
            public int clamp;
            public int c_acc;
            public int foward;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] reserve = new int[8];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBHPAC
        {
            public IODBHPAC_tune tune1 = new IODBHPAC_tune();
            public IODBHPAC_tune tune2 = new IODBHPAC_tune();
            public IODBHPAC_tune tune3 = new IODBHPAC_tune();
        }

        /* cnc_rd3dtooltip:read tip of tool for 3D handle */
        /* cnc_rd3dmovrlap:read move overrlap of tool for 3D handle */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DHDL_data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] axes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] data;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DHDL
        {
            public ODB3DHDL_data data1 = new ODB3DHDL_data();
            public ODB3DHDL_data data2 = new ODB3DHDL_data();
        }

        /* cnc_rd3dpulse:read pulse for 3D handle */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DPLS_data
        {
            public int right_angle_x;
            public int right_angle_y;
            public int tool_axis;
            public int tool_tip_a_b;
            public int tool_tip_c;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODB3DPLS
        {
            public ODB3DPLS_data pls1 = new ODB3DPLS_data();
            public ODB3DPLS_data pls2 = new ODB3DPLS_data();
        }

        /* cnc_rdaxisname: read axis name */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAXISNAME_data
        {
            public byte name;          /* axis name */
            public byte suff;          /* suffix */
        }
#if M_AXIS2
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBAXISNAME
    {
        public ODBAXISNAME_data data1 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data2 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data3 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data4 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data5 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data6 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data7 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data8 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data9 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data10= new ODBAXISNAME_data();
        public ODBAXISNAME_data data11= new ODBAXISNAME_data();
        public ODBAXISNAME_data data12= new ODBAXISNAME_data();
        public ODBAXISNAME_data data13= new ODBAXISNAME_data();
        public ODBAXISNAME_data data14= new ODBAXISNAME_data();
        public ODBAXISNAME_data data15= new ODBAXISNAME_data();
        public ODBAXISNAME_data data16= new ODBAXISNAME_data();
        public ODBAXISNAME_data data17= new ODBAXISNAME_data();
        public ODBAXISNAME_data data18= new ODBAXISNAME_data();
        public ODBAXISNAME_data data19= new ODBAXISNAME_data();
        public ODBAXISNAME_data data20= new ODBAXISNAME_data();
        public ODBAXISNAME_data data21= new ODBAXISNAME_data();
        public ODBAXISNAME_data data22= new ODBAXISNAME_data();
        public ODBAXISNAME_data data23= new ODBAXISNAME_data();
        public ODBAXISNAME_data data24= new ODBAXISNAME_data();
    }
#elif FS15D
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBAXISNAME
    {
        public ODBAXISNAME_data data1 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data2 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data3 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data4 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data5 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data6 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data7 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data8 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data9 = new ODBAXISNAME_data();
        public ODBAXISNAME_data data10= new ODBAXISNAME_data();
    }
#else
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBAXISNAME
        {
            public ODBAXISNAME_data data1 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data2 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data3 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data4 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data5 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data6 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data7 = new ODBAXISNAME_data();
            public ODBAXISNAME_data data8 = new ODBAXISNAME_data();
        }
#endif

        /* cnc_rdspdlname: read spindle name */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDLNAME_data
        {
            public byte name;          /* spindle name */
            public byte suff1;         /* suffix */
            public byte suff2;         /* suffix */
            public byte suff3;         /* suffix */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSPDLNAME
        {
            public ODBSPDLNAME_data data1 = new ODBSPDLNAME_data();
            public ODBSPDLNAME_data data2 = new ODBSPDLNAME_data();
            public ODBSPDLNAME_data data3 = new ODBSPDLNAME_data();
            public ODBSPDLNAME_data data4 = new ODBSPDLNAME_data();
        }

        /* cnc_exaxisname: read extended axis name */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBEXAXISNAME
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname1 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname2 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname3 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname4 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname5 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname6 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname7 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname8 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname9 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname10 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname11 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname12 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname13 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname14 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname15 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname16 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname17 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname18 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname19 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname20 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname21 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname22 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname23 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname24 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname25 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname26 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname27 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname28 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname29 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname30 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname31 = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string axname32 = new string(' ', 4);
        }

        /* cnc_wrunsolicprm: Set the unsolicited message parameters */
        /* cnc_rdunsolicprm: Get the unsolicited message parameters */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IODBUNSOLIC_pmc
        {
            public short type;
            public short rdaddr;
            public short rdno;
            public short rdsize;
            private int dummy;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBUNSOLIC
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string ipaddr = new string(' ', 16);
            public ushort port;
            public short reqaddr;
            public short pmcno;
            public short retry;
            public short timeout;
            public short alivetime;
            public short setno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public IODBUNSOLIC_pmc[] rddata = new IODBUNSOLIC_pmc[3];
        }

        /* cnc_rdunsolicmsg: Reads the unsolicited message data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IDBUNSOLICMSG_msg
        {
            public short rdsize;
            public IntPtr data;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBUNSOLICMSG
        {
            public short getno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public IDBUNSOLICMSG_msg[] msg = new IDBUNSOLICMSG_msg[3];
        }

        /* cnc_rdpm_cncitem: read cnc maintenance item */
        /* cnc_rdpm_mcnitem: read machine specific maintenance item */
        /* cnc_wrpm_mcnitem: write machine specific maintenance item */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBITEM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name1 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name2 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name3 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name4 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name5 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name6 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name7 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name8 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name9 = new string(' ', 62);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name10 = new string(' ', 62);
        }

        /* cnc_rdpm_item:read maintenance item status */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMAINTE_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 62)]
            public string name = new string(' ', 62); /* name */
            public int type;        /* life count type */
            public int total;       /* total life time (minite basis) */
            public int remain;      /* life rest time */
            public int stat;        /* life state */
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMAINTE
        {
            public IODBPMAINTE_data data1 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data2 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data3 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data4 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data5 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data6 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data7 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data8 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data9 = new IODBPMAINTE_data();
            public IODBPMAINTE_data data10 = new IODBPMAINTE_data();
        }

        /* cnc_sysinfo_ex:read CNC system path information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSEX_path
        {
            public short system;
            public short group;
            public short attrib;
            public short ctrl_axis;
            public short ctrl_srvo;
            public short ctrl_spdl;
            public short mchn_no;
            public short reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSEX_data
        {
            public ODBSYSEX_path data1 = new ODBSYSEX_path();
            public ODBSYSEX_path data2 = new ODBSYSEX_path();
            public ODBSYSEX_path data3 = new ODBSYSEX_path();
            public ODBSYSEX_path data4 = new ODBSYSEX_path();
            public ODBSYSEX_path data5 = new ODBSYSEX_path();
            public ODBSYSEX_path data6 = new ODBSYSEX_path();
            public ODBSYSEX_path data7 = new ODBSYSEX_path();
            public ODBSYSEX_path data8 = new ODBSYSEX_path();
            public ODBSYSEX_path data9 = new ODBSYSEX_path();
            public ODBSYSEX_path data10 = new ODBSYSEX_path();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSYSEX
        {
            public short max_axis;
            public short max_spdl;
            public short max_path;
            public short max_mchn;
            public short ctrl_axis;
            public short ctrl_srvo;
            public short ctrl_spdl;
            public short ctrl_path;
            public short ctrl_mchn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] reserved = new short[3];
            public ODBSYSEX_data path = new ODBSYSEX_data();
        }

        /*------------------*/
        /* CNC : SERCOS I/F */
        /*------------------*/

        /* cnc_srcsrdidinfo:Read ID information of SERCOS I/F */
        /* cnc_srcswridinfo:Write ID information of SERCOS I/F */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class IODBIDINF
        {
            public int id_no;
            public short drv_no;
            public short acc_element;
            public short err_general;
            public short err_id_no;
            public short err_id_name;
            public short err_attr;
            public short err_unit;
            public short err_min_val;
            public short err_max_val;
            public short id_name_len;
            public short id_name_max;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
            public string id_name = new string(' ', 60);
            public int attr;
            public short unit_len;
            public short unit_max;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] unit = new byte[12];
            public int min_val;
            public int max_val;
        }

        /* cnc_srcsrdexstat:Get execution status of reading/writing operation data of SERCOS I/F */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSRCSST
        {
            public short acc_element;
            public short err_general;
            public short err_id_no;
            public short err_attr;
            public short err_op_data;
        }

        /* cnc_srcsrdlayout:Read drive assign of SERCOS I/F */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBSRCSLYT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] spndl;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] servo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string axis_name = new string(' ', 8);
        }

        /*----------------------------*/
        /* CNC : Servo Guide          */
        /*----------------------------*/
        /* cnc_sdsetchnl:Servo Guide (Channel data set) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBCHAN_data
        {
            public byte chno;
            public sbyte axis;
            public int datanum;
            public ushort datainf;
            public short dataadr;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBCHAN
        {
            public IDBCHAN_data data1 = new IDBCHAN_data();
            public IDBCHAN_data data2 = new IDBCHAN_data();
            public IDBCHAN_data data3 = new IDBCHAN_data();
            public IDBCHAN_data data4 = new IDBCHAN_data();
            public IDBCHAN_data data5 = new IDBCHAN_data();
            public IDBCHAN_data data6 = new IDBCHAN_data();
            public IDBCHAN_data data7 = new IDBCHAN_data();
            public IDBCHAN_data data8 = new IDBCHAN_data();
        }

        /* cnc_sdsetchnl:Servo Guide (read Sampling data) */
        /* cnc_sfbreadsmpl:Servo feedback data (read Sampling data) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSD
        {
            public IntPtr chadata;
            public IntPtr count;
        }

        /* cnc_sfbsetchnl:Servo feedback data (Channel data set) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IDBSFBCHAN
        {
            public byte chno;
            public sbyte axis;
            public ushort shift;
        }


        /*-------------------------*/
        /* CNC : FS18-LN function  */
        /*-------------------------*/

        /* cnc_allowcnd:read allowanced state */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBCAXIS
        {
            public short dummy;             /* dummy */
            public short type;              /* axis number */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
            public sbyte[] data;              /* data value */
        }


        /*---------------------------------*/
        /* CNC : C-EXE SRAM file function  */
        /*---------------------------------*/

        /* read C-EXE SRAM disk directory */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class CFILEINFO_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string fname = new string(' ', 12); /* file name */
            public int file_size; /* file size (bytes) */
            public int file_attr; /* attribute */
            public short year;       /* year */
            public short month;      /* month */
            public short day;        /* day */
            public short hour;       /* hour */
            public short minute;     /* mimute */
            public short second;     /* second */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class CFILEINFO
        {
            public CFILEINFO_data data1 = new CFILEINFO_data();
            public CFILEINFO_data data2 = new CFILEINFO_data();
            public CFILEINFO_data data3 = new CFILEINFO_data();
            public CFILEINFO_data data4 = new CFILEINFO_data();
            public CFILEINFO_data data5 = new CFILEINFO_data();
            public CFILEINFO_data data6 = new CFILEINFO_data();
            public CFILEINFO_data data7 = new CFILEINFO_data();
            public CFILEINFO_data data8 = new CFILEINFO_data();
            public CFILEINFO_data data9 = new CFILEINFO_data();
            public CFILEINFO_data data10 = new CFILEINFO_data();
        }

        /*-----*/
        /* PMC */
        /*-----*/

        /* pmc_rdpmcrng:read PMC data(area specified) */
        /* pmc_wrpmcrng:write PMC data(area specified) */
        [StructLayout(LayoutKind.Explicit)]
        public class IODBPMC0
        {
            [FieldOffset(0)]
            public short type_a;    /* PMC address type */
            [FieldOffset(2)]
            public short type_d;    /* PMC data type */
            [FieldOffset(4)]
            public short datano_s;  /* start PMC address */
            [FieldOffset(6)]
            public short datano_e;  /* end PMC address */
            [FieldOffset(8),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] cdata;       /* PMC data */
        } /* In case that the number of data is 5 */

        [StructLayout(LayoutKind.Explicit)]
        public class IODBPMC1
        {
            [FieldOffset(0)]
            public short type_a;    /* PMC address type */
            [FieldOffset(2)]
            public short type_d;    /* PMC data type */
            [FieldOffset(4)]
            public short datano_s;  /* start PMC address */
            [FieldOffset(6)]
            public short datano_e;  /* end PMC address */
            [FieldOffset(8),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] idata;
        } /* In case that the number of data is 5 */

        [StructLayout(LayoutKind.Explicit)]
        public class IODBPMC2
        {
            [FieldOffset(0)]
            public short type_a;    /* PMC address type */
            [FieldOffset(2)]
            public short type_d;    /* PMC data type */
            [FieldOffset(4)]
            public short datano_s;  /* start PMC address */
            [FieldOffset(6)]
            public short datano_e;  /* end PMC address */
            [FieldOffset(8),
            MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] ldata;
        } /* In case that the number of data is 5 */

        /* pmc_rdpmcinfo:read informations of PMC data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCINF_info
        {
            public char pmc_adr;
            public byte adr_attr;
            public ushort top_num;
            public ushort last_num;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCINF1
        {
            public ODBPMCINF_info info1 = new ODBPMCINF_info();
            public ODBPMCINF_info info2 = new ODBPMCINF_info();
            public ODBPMCINF_info info3 = new ODBPMCINF_info();
            public ODBPMCINF_info info4 = new ODBPMCINF_info();
            public ODBPMCINF_info info5 = new ODBPMCINF_info();
            public ODBPMCINF_info info6 = new ODBPMCINF_info();
            public ODBPMCINF_info info7 = new ODBPMCINF_info();
            public ODBPMCINF_info info8 = new ODBPMCINF_info();
            public ODBPMCINF_info info9 = new ODBPMCINF_info();
            public ODBPMCINF_info info10 = new ODBPMCINF_info();
            public ODBPMCINF_info info11 = new ODBPMCINF_info();
            public ODBPMCINF_info info12 = new ODBPMCINF_info();
            public ODBPMCINF_info info13 = new ODBPMCINF_info();
            public ODBPMCINF_info info14 = new ODBPMCINF_info();
            public ODBPMCINF_info info15 = new ODBPMCINF_info();
            public ODBPMCINF_info info16 = new ODBPMCINF_info();
            public ODBPMCINF_info info17 = new ODBPMCINF_info();
            public ODBPMCINF_info info18 = new ODBPMCINF_info();
            public ODBPMCINF_info info19 = new ODBPMCINF_info();
            public ODBPMCINF_info info20 = new ODBPMCINF_info();
            public ODBPMCINF_info info21 = new ODBPMCINF_info();
            public ODBPMCINF_info info22 = new ODBPMCINF_info();
            public ODBPMCINF_info info23 = new ODBPMCINF_info();
            public ODBPMCINF_info info24 = new ODBPMCINF_info();
            public ODBPMCINF_info info25 = new ODBPMCINF_info();
            public ODBPMCINF_info info26 = new ODBPMCINF_info();
            public ODBPMCINF_info info27 = new ODBPMCINF_info();
            public ODBPMCINF_info info28 = new ODBPMCINF_info();
            public ODBPMCINF_info info29 = new ODBPMCINF_info();
            public ODBPMCINF_info info30 = new ODBPMCINF_info();
            public ODBPMCINF_info info31 = new ODBPMCINF_info();
            public ODBPMCINF_info info32 = new ODBPMCINF_info();
            public ODBPMCINF_info info33 = new ODBPMCINF_info();
            public ODBPMCINF_info info34 = new ODBPMCINF_info();
            public ODBPMCINF_info info35 = new ODBPMCINF_info();
            public ODBPMCINF_info info36 = new ODBPMCINF_info();
            public ODBPMCINF_info info37 = new ODBPMCINF_info();
            public ODBPMCINF_info info38 = new ODBPMCINF_info();
            public ODBPMCINF_info info39 = new ODBPMCINF_info();
            public ODBPMCINF_info info40 = new ODBPMCINF_info();
            public ODBPMCINF_info info41 = new ODBPMCINF_info();
            public ODBPMCINF_info info42 = new ODBPMCINF_info();
            public ODBPMCINF_info info43 = new ODBPMCINF_info();
            public ODBPMCINF_info info44 = new ODBPMCINF_info();
            public ODBPMCINF_info info45 = new ODBPMCINF_info();
            public ODBPMCINF_info info46 = new ODBPMCINF_info();
            public ODBPMCINF_info info47 = new ODBPMCINF_info();
            public ODBPMCINF_info info48 = new ODBPMCINF_info();
            public ODBPMCINF_info info49 = new ODBPMCINF_info();
            public ODBPMCINF_info info50 = new ODBPMCINF_info();
            public ODBPMCINF_info info51 = new ODBPMCINF_info();
            public ODBPMCINF_info info52 = new ODBPMCINF_info();
            public ODBPMCINF_info info53 = new ODBPMCINF_info();
            public ODBPMCINF_info info54 = new ODBPMCINF_info();
            public ODBPMCINF_info info55 = new ODBPMCINF_info();
            public ODBPMCINF_info info56 = new ODBPMCINF_info();
            public ODBPMCINF_info info57 = new ODBPMCINF_info();
            public ODBPMCINF_info info58 = new ODBPMCINF_info();
            public ODBPMCINF_info info59 = new ODBPMCINF_info();
            public ODBPMCINF_info info60 = new ODBPMCINF_info();
            public ODBPMCINF_info info61 = new ODBPMCINF_info();
            public ODBPMCINF_info info62 = new ODBPMCINF_info();
            public ODBPMCINF_info info63 = new ODBPMCINF_info();
            public ODBPMCINF_info info64 = new ODBPMCINF_info();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCINF
        {
            public short datano;
            public ODBPMCINF1 info = new ODBPMCINF1();
        }

        /* pmc_rdcntldata:read PMC parameter data table control data */
        /* pmc_wrcntldata:write PMC parameter data table control data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMCCNTL_info
        {
            public byte tbl_prm;
            public byte data_type;
            public ushort data_size;
            public ushort data_dsp;
            public short dummy;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMCCNTL1
        {
            public IODBPMCCNTL_info info1 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info2 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info3 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info4 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info5 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info6 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info7 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info8 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info9 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info10 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info11 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info12 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info13 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info14 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info15 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info16 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info17 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info18 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info19 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info20 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info21 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info22 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info23 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info24 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info25 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info26 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info27 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info28 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info29 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info30 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info31 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info32 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info33 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info34 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info35 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info36 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info37 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info38 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info39 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info40 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info41 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info42 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info43 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info44 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info45 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info46 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info47 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info48 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info49 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info50 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info51 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info52 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info53 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info54 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info55 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info56 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info57 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info58 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info59 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info60 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info61 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info62 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info63 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info64 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info65 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info66 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info67 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info68 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info69 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info70 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info71 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info72 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info73 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info74 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info75 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info76 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info77 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info78 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info79 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info80 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info81 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info82 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info83 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info84 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info85 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info86 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info87 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info88 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info89 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info90 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info91 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info92 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info93 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info94 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info95 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info96 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info97 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info98 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info99 = new IODBPMCCNTL_info();
            public IODBPMCCNTL_info info100 = new IODBPMCCNTL_info();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMCCNTL
        {
            public short datano_s;
            public short dummy;
            public short datano_e;
            public IODBPMCCNTL1 info = new IODBPMCCNTL1();
        }

        /* pmc_rdalmmsg:read PMC alarm message */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPMCALM_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string almmsg = new string(' ', 128); /* alarm message */
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCALM
        {
            public ODBPMCALM_data msg1 = new ODBPMCALM_data();
            public ODBPMCALM_data msg2 = new ODBPMCALM_data();
            public ODBPMCALM_data msg3 = new ODBPMCALM_data();
            public ODBPMCALM_data msg4 = new ODBPMCALM_data();
            public ODBPMCALM_data msg5 = new ODBPMCALM_data();
            public ODBPMCALM_data msg6 = new ODBPMCALM_data();
            public ODBPMCALM_data msg7 = new ODBPMCALM_data();
            public ODBPMCALM_data msg8 = new ODBPMCALM_data();
            public ODBPMCALM_data msg9 = new ODBPMCALM_data();
            public ODBPMCALM_data msg10 = new ODBPMCALM_data();
        } /* In case that the number of data is 10 */

        /* pmc_getdtailerr:get detail error for pmc */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCERR
        {
            public short err_no;
            public short err_dtno;
        }

        /* pmc_rdpmctitle:read pmc title data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPMCTITLE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string mtb = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string machine = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string type = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string prgno = new string(' ', 8);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string prgvers = new string(' ', 4);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string prgdraw = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string date = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string design = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string written = new string(' ', 48);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string remarks = new string(' ', 48);
        }

        /* pmc_rdpmcrng_ext:read PMC data */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPMCEXT
        {
            public short type_a;    /* PMC address type */
            public short type_d;    /* PMC data type */
            public short datano_s;  /* start PMC address */
            public short datano_e;  /* end PMC address */
            public short err_code;  /* error code */
            public short reserved;  /* reserved */
            [MarshalAs(UnmanagedType.AsAny)]
            public object data;      /* pointer to buffer */
        }

        /* pmc_rdpmcaddr:read PMC address information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCADR_info
        {
            public byte pmc_adr;
            public byte adr_attr;
            public ushort offset;
            public ushort top;
            public ushort num;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCADR1
        {
            public ODBPMCADR_info info1 = new ODBPMCADR_info();
            public ODBPMCADR_info info2 = new ODBPMCADR_info();
            public ODBPMCADR_info info3 = new ODBPMCADR_info();
            public ODBPMCADR_info info4 = new ODBPMCADR_info();
            public ODBPMCADR_info info5 = new ODBPMCADR_info();
            public ODBPMCADR_info info6 = new ODBPMCADR_info();
            public ODBPMCADR_info info7 = new ODBPMCADR_info();
            public ODBPMCADR_info info8 = new ODBPMCADR_info();
            public ODBPMCADR_info info9 = new ODBPMCADR_info();
            public ODBPMCADR_info info10 = new ODBPMCADR_info();
            public ODBPMCADR_info info11 = new ODBPMCADR_info();
            public ODBPMCADR_info info12 = new ODBPMCADR_info();
            public ODBPMCADR_info info13 = new ODBPMCADR_info();
            public ODBPMCADR_info info14 = new ODBPMCADR_info();
            public ODBPMCADR_info info15 = new ODBPMCADR_info();
            public ODBPMCADR_info info16 = new ODBPMCADR_info();
            public ODBPMCADR_info info17 = new ODBPMCADR_info();
            public ODBPMCADR_info info18 = new ODBPMCADR_info();
            public ODBPMCADR_info info19 = new ODBPMCADR_info();
            public ODBPMCADR_info info20 = new ODBPMCADR_info();
            public ODBPMCADR_info info21 = new ODBPMCADR_info();
            public ODBPMCADR_info info22 = new ODBPMCADR_info();
            public ODBPMCADR_info info23 = new ODBPMCADR_info();
            public ODBPMCADR_info info24 = new ODBPMCADR_info();
            public ODBPMCADR_info info25 = new ODBPMCADR_info();
            public ODBPMCADR_info info26 = new ODBPMCADR_info();
            public ODBPMCADR_info info27 = new ODBPMCADR_info();
            public ODBPMCADR_info info28 = new ODBPMCADR_info();
            public ODBPMCADR_info info29 = new ODBPMCADR_info();
            public ODBPMCADR_info info30 = new ODBPMCADR_info();
            public ODBPMCADR_info info31 = new ODBPMCADR_info();
            public ODBPMCADR_info info32 = new ODBPMCADR_info();
            public ODBPMCADR_info info33 = new ODBPMCADR_info();
            public ODBPMCADR_info info34 = new ODBPMCADR_info();
            public ODBPMCADR_info info35 = new ODBPMCADR_info();
            public ODBPMCADR_info info36 = new ODBPMCADR_info();
            public ODBPMCADR_info info37 = new ODBPMCADR_info();
            public ODBPMCADR_info info38 = new ODBPMCADR_info();
            public ODBPMCADR_info info39 = new ODBPMCADR_info();
            public ODBPMCADR_info info40 = new ODBPMCADR_info();
            public ODBPMCADR_info info41 = new ODBPMCADR_info();
            public ODBPMCADR_info info42 = new ODBPMCADR_info();
            public ODBPMCADR_info info43 = new ODBPMCADR_info();
            public ODBPMCADR_info info44 = new ODBPMCADR_info();
            public ODBPMCADR_info info45 = new ODBPMCADR_info();
            public ODBPMCADR_info info46 = new ODBPMCADR_info();
            public ODBPMCADR_info info47 = new ODBPMCADR_info();
            public ODBPMCADR_info info48 = new ODBPMCADR_info();
            public ODBPMCADR_info info49 = new ODBPMCADR_info();
            public ODBPMCADR_info info50 = new ODBPMCADR_info();
            public ODBPMCADR_info info51 = new ODBPMCADR_info();
            public ODBPMCADR_info info52 = new ODBPMCADR_info();
            public ODBPMCADR_info info53 = new ODBPMCADR_info();
            public ODBPMCADR_info info54 = new ODBPMCADR_info();
            public ODBPMCADR_info info55 = new ODBPMCADR_info();
            public ODBPMCADR_info info56 = new ODBPMCADR_info();
            public ODBPMCADR_info info57 = new ODBPMCADR_info();
            public ODBPMCADR_info info58 = new ODBPMCADR_info();
            public ODBPMCADR_info info59 = new ODBPMCADR_info();
            public ODBPMCADR_info info60 = new ODBPMCADR_info();
            public ODBPMCADR_info info61 = new ODBPMCADR_info();
            public ODBPMCADR_info info62 = new ODBPMCADR_info();
            public ODBPMCADR_info info63 = new ODBPMCADR_info();
            public ODBPMCADR_info info64 = new ODBPMCADR_info();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBPMCADR
        {
            public uint io_adr;
            public short datano;
            public ODBPMCADR1 info = new ODBPMCADR1();
        }


        /*--------------------------*/
        /* PROFIBUS function        */
        /*--------------------------*/

        /* pmc_prfrdconfig:read PROFIBUS configration data */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBPRFCNF
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string master_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
            public string master_ver = new string(' ', 3);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string slave_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
            public string slave_ver = new string(' ', 3);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string cntl_ser = new string(' ', 5);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
            public string cntl_ver = new string(' ', 3);
        }

        /* pmc_prfrdbusprm:read bus parameter for master function */
        /* pmc_prfwrbusprm:write bus parameter for master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBBUSPRM
        {
            public sbyte fdl_add;
            public sbyte baudrate;
            public ushort tsl;
            public ushort min_tsdr;
            public ushort max_tsdr;
            public byte tqui;
            public byte tset;
            public int ttr;
            public sbyte gap;
            public sbyte hsa;
            public sbyte max_retry;
            public byte bp_flag;
            public ushort min_slv_int;
            public ushort poll_tout;
            public ushort data_cntl;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] reserve1 = new byte[6];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cls2_name = new byte[32];
            public short user_dlen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 62)]
            public byte[] user_data = new byte[62];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] reserve2 = new byte[96];
        }

        /* pmc_prfrdslvprm:read slave parameter for master function */
        /* pmc_prfwrslvprm:write slave parameter for master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLVPRM
        {
            public short dis_enb;
            public ushort ident_no;
            public byte slv_flag;
            public byte slv_type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] reserve1 = new byte[12];
            public byte slv_stat;
            public byte wd_fact1;
            public byte wd_fact2;
            public byte min_tsdr;
            public char reserve2;
            public byte grp_ident;
            public short user_plen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] user_pdata = new byte[32];
            public short cnfg_dlen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
            public byte[] cnfg_data = new byte[126];
            public short slv_ulen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] slv_udata = new byte[30];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] reserve3 = new byte[8];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLVPRM2
        {
            public short dis_enb;
            public ushort ident_no;
            public byte slv_flag;
            public byte slv_type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] reserve1 = new byte[12];
            public byte slv_stat;
            public byte wd_fact1;
            public byte wd_fact2;
            public byte min_tsdr;
            public sbyte reserve2;
            public byte grp_ident;
            public short user_plen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 206)]
            public byte[] user_pdata = new byte[206];
            public short cnfg_dlen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
            public byte[] cnfg_data = new byte[126];
            public short slv_ulen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] slv_udata = new byte[30];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] reserve3 = new byte[8];
        }

        /* pmc_prfrdallcadr:read allocation address for master function */
        /* pmc_prfwrallcadr:set allocation address for master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBPRFADR
        {
            public byte di_size;
            public byte di_type;
            public ushort di_addr;
            public short reserve1;
            public byte do_size;
            public byte do_type;
            public ushort do_addr;
            public short reserve2;
            public byte dgn_size;
            public byte dgn_type;
            public ushort dgn_addr;
        }

        /* pmc_prfrdslvaddr:read allocation address for slave function */
        /* pmc_prfwrslvaddr:set allocation address for slave function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLVADR
        {
            public byte slave_no;
            public byte di_size;
            public byte di_type;
            public ushort di_addr;
            public byte do_size;
            public byte do_type;
            public ushort do_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] reserve = new byte[7];
        }

        /* pmc_prfrdslvstat:read status for slave function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBSLVST
        {
            public byte cnfg_stat;
            public byte prm_stat;
            public sbyte wdg_stat;
            public byte live_stat;
            public short ident_no;
        }

        /* pmc_prfrdslvid:Reads slave index data of master function */
        /* pmc_prfwrslvid:Writes slave index data of master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLVID
        {
            public short dis_enb;
            public short slave_no;
            public short nsl;
            public byte dgn_size;
            public char dgn_type;
            public ushort dgn_addr;
        }

        /* pmc_prfrdslvprm2:Reads  slave parameter of master function(2) */
        /* pmc_prfwrslvprm2:Writes slave parameter of master function(2) */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBSLVPRM3
        {
            public ushort ident_no;
            public byte slv_flag;
            public byte slv_type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] reserve1 = new char[12];
            public byte slv_stat;
            public byte wd_fact1;
            public byte wd_fact2;
            public byte min_tsdr;
            public char reserve2;
            public byte grp_ident;
            public short user_plen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 206)]
            public char[] user_pdata = new char[206];
            public short slv_ulen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public char[] slv_udata = new char[30];
        }

        /* pmc_prfrddido:Reads DI/DO parameter of master function */
        /* pmc_prfwrdido:Writes DI/DO parameter of master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBDIDO
        {
            public short slave_no;
            public short slot_no;
            public byte di_size;
            public char di_type;
            public ushort di_addr;
            public byte do_size;
            public char do_type;
            public ushort do_addr;
            public short shift;
            public byte module_dlen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] module_data = new char[128];
        }

        /* pmc_prfrdindiadr:Reads indication address of master function */
        /* pmc_prfwrindiadr:Writes indication address of master function */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBINDEADR
        {
            public byte dummy;
            public char indi_type;
            public ushort indi_addr;
        }

        /*-----------------------------------------------*/
        /* DS : Data server & Ethernet board function    */
        /*-----------------------------------------------*/

        /* etb_rdparam : read丂the parameter of the Ethernet board  */
        /* etb_wrparam : write the parameter of the Ethernet board  */

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class TCPPRM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string OwnIPAddress = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string SubNetMask = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string RouterIPAddress = new string(' ', 16);
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class HOSTPRM
        {
            public short DataServerPort;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string DataServerIPAddress = new string(' ', 16);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DataServerUserName = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DataServerPassword = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DataServerLoginDirectory = new string(' ', 128);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class FTPPRM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FTPServerUserName = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FTPServerPassword = new string(' ', 32);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string FTPServerLoginDirectory = new string(' ', 128);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ETBPRM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string OwnMACAddress = new string(' ', 128);
            public short MaximumChannel;
            public short HDDExistence;
            public short NumberOfScreens;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBETP
        {
            public short Dummy_ParameterType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 210)]
            public byte[] prm = new byte[210];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBETP_TCP
        {
            public short ParameterType;
            public TCPPRM tcp;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBETP_HOST
        {
            public short ParameterType;
            public HOSTPRM host;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBETP_FTP
        {
            public short ParameterType;
            public FTPPRM ftp;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class IODBETP_ETB
        {
            public short ParameterType;
            public ETBPRM etb;
        }

        /* etb_rderrmsg : read the error message of the Ethernet board */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBETMSG
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string title = new string(' ', 33);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 390)]
            public string message = new string(' ', 390);
        }

        /* ds_rdhddinfo : read information of the Data Server's HDD */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHDDINF
        {
            public int file_num;
            public int remainder_l;
            public int remainder_h;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] current_dir = new char[32];
        }

        /* ds_rdhdddir : read the file list of the Data Server's HDD */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHDDDIR_data
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string file_name = new string(' ', 64);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string comment = new string(' ', 80);
            public short attribute;
            public short reserved;
            public int size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string date = new string(' ', 16);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHDDDIR
        {
            public ODBHDDDIR_data data1 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data2 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data3 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data4 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data5 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data6 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data7 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data8 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data9 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data10 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data11 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data12 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data13 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data14 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data15 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data16 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data17 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data18 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data19 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data20 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data21 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data22 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data23 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data24 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data25 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data26 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data27 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data28 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data29 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data30 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data31 = new ODBHDDDIR_data();
            public ODBHDDDIR_data data32 = new ODBHDDDIR_data();
        }

        /* ds_rdhostdir : read the file list of the host */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHOSTDIR_data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] host_file = new char[128];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class ODBHOSTDIR
        {
            public ODBHOSTDIR_data data1 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data2 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data3 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data4 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data5 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data6 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data7 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data8 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data9 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data10 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data11 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data12 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data13 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data14 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data15 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data16 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data17 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data18 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data19 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data20 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data21 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data22 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data23 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data24 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data25 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data26 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data27 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data28 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data29 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data30 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data31 = new ODBHOSTDIR_data();
            public ODBHOSTDIR_data data32 = new ODBHOSTDIR_data();
        }

        /* ds_rdmntinfo : read maintenance information */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class DSMNTINFO
        {
            public ushort empty_cnt;
            public uint total_size;
            public ushort ReadPtr;
            public ushort WritePtr;
        };

        /*--------------------------*/
        /* HSSB multiple connection */
        /*--------------------------*/

        /* cnc_rdnodeinfo:read node informations */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public class ODBNODE
        {
            public int node_no;
            public int io_base;
            public int status;
            public int cnc_type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string node_name = new string(' ', 20);
        }


        /*-------------------------------------*/
        /* CNC: Control axis / spindle related */
        /*-------------------------------------*/

        /* read actual axis feedrate(F) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_actf")]
        public static extern short cnc_actf(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBACT a);

        /* read absolute axis position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_absolute")]
        public static extern short cnc_absolute(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read machine axis position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_machine")]
        public static extern short cnc_machine(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read relative axis position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_relative")]
        public static extern short cnc_relative(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read distance to go */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_distance")]
        public static extern short cnc_distance(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read skip position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_skip")]
        public static extern short cnc_skip(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read servo delay value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srvdelay")]
        public static extern short cnc_srvdelay(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read acceleration/deceleration delay value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_accdecdly")]
        public static extern short cnc_accdecdly(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read all dynamic data */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddynamic")]
        public static extern short cnc_rddynamic(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDY_1 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddynamic")]
        public static extern short cnc_rddynamic(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDY_2 c);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rddynamico8")]
    public static extern short cnc_rddynamic(ushort FlibHndl,
        short a, short b, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBDY_1 c);
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rddynamico8")]
    public static extern short cnc_rddynamic(ushort FlibHndl,
        short a, short b, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBDY_2 c);
#endif

        /* read all dynamic data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddynamic2")]
        public static extern short cnc_rddynamic2(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDY2_1 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddynamic2")]
        public static extern short cnc_rddynamic2(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDY2_2 c);

        /* read actual spindle speed(S) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_acts")]
        public static extern short cnc_acts(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBACT a);

        /* read actual spindle speed(S) (All or spesified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_acts2")]
        public static extern short cnc_acts2(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBACT2 b);

        /* set origin / preset relative axis position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrrelpos")]
        public static extern short cnc_wrrelpos(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBWRR b);

        /* preset work coordinate */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_prstwkcd")]
        public static extern short cnc_prstwkcd(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBWRA b);

        /* read manual overlapped motion value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmovrlap")]
        public static extern short cnc_rdmovrlap(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBOVL c);

        /* cancel manual overlapped motion value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_canmovrlap")]
        public static extern short cnc_canmovrlap(ushort FlibHndl, short a);

        /* read load information of serial spindle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspload")]
        public static extern short cnc_rdspload(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPN b);

        /* read maximum r.p.m. ratio of serial spindle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspmaxrpm")]
        public static extern short cnc_rdspmaxrpm(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPN b);

        /* read gear ratio of serial spindle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspgear")]
        public static extern short cnc_rdspgear(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPN b);

        /* read absolute axis position 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_absolute2")]
        public static extern short cnc_absolute2(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read relative axis position 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_relative2")]
        public static extern short cnc_relative2(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* set wire vertival position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setvrtclpos")]
        public static extern short cnc_setvrtclpos(ushort FlibHndl, short a);

        /* set wire threading position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setthrdngpos")]
        public static extern short cnc_setthrdngpos(ushort FlibHndl);

        /* read tool position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdposition")]
        public static extern short cnc_rdposition(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPOS c);

        /* read current speed */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspeed")]
        public static extern short cnc_rdspeed(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPEED b);

        /* read servo load meter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsvmeter")]
        public static extern short cnc_rdsvmeter(ushort FlibHndl,
            ref short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSVLOAD b);

        /* read spindle load meter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspmeter")]
        public static extern short cnc_rdspmeter(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPLOAD c);

        /* read manual feed for 5-axis machining */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd5axmandt")]
        public static extern short cnc_rd5axmandt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB5AXMAN a);

        /* read amount of machine axes movement of manual feed for 5-axis machining */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd5axovrlap")]
        public static extern short cnc_rd5axovrlap(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read handle interruption */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhndintrpt")]
        public static extern short cnc_rdhndintrpt(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHND c);

        /* clear pulse values of manual feed for 5-axis machining */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clr5axpls")]
        public static extern short cnc_clr5axpls(ushort FlibHndl, short a);

        /* read constant surface speed */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspcss")]
        public static extern short cnc_rdspcss(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBCSS a);

        /* read execution program pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdexecpt")]
        public static extern short cnc_rdexecpt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGPNT a, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGPNT b);

        /* read various axis data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdaxisdata")]
        public static extern short cnc_rdaxisdata(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, short c, ref short d, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXDT e);

        /*----------------------*/
        /* CNC: Program related */
        /*----------------------*/

        /* start downloading NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnstart")]
        public static extern short cnc_dwnstart(ushort FlibHndl);

        /* download NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_download")]
        public static extern short cnc_download(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b);

        /* download NC program(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cdownload")]
        public static extern short cnc_cdownload(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b);

        /* end of downloading NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnend")]
        public static extern short cnc_dwnend(ushort FlibHndl);

        /* end of downloading NC program 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnend2")]
        public static extern short cnc_dwnend2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* start downloading NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnstart3")]
        public static extern short cnc_dwnstart3(ushort FlibHndl, short a);

        /* start downloading NC program 3 special */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnstart3_f")]
        public static extern short cnc_dwnstart3_f(ushort FlibHndl,
            short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, [In, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* download NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_download3")]
        public static extern short cnc_download3(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of downloading NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnend3")]
        public static extern short cnc_dwnend3(ushort FlibHndl);

        /* start downloading NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnstart4")]
        public static extern short cnc_dwnstart4(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* download NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_download4")]
        public static extern short cnc_download4(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of downloading NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dwnend4")]
        public static extern short cnc_dwnend4(ushort FlibHndl);

        /* start verification of NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_vrfstart")]
        public static extern short cnc_vrfstart(ushort FlibHndl);

        /* verify NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_verify")]
        public static extern short cnc_verify(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b);

        /* verify NC program(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cverify")]
        public static extern short cnc_cverify(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b);

        /* end of verification */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_vrfend")]
        public static extern short cnc_vrfend(ushort FlibHndl);

        /* start verification of NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_vrfstart4")]
        public static extern short cnc_vrfstart4(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* verify NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_verify4")]
        public static extern short cnc_verify4(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of verification */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_vrfend4")]
        public static extern short cnc_vrfend4(ushort FlibHndl);

        /* start downloading DNC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dncstart")]
        public static extern short cnc_dncstart(ushort FlibHndl);

        /* download DNC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dnc")]
        public static extern short cnc_dnc(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, ushort b);

        /* download DNC program(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cdnc")]
        public static extern short cnc_cdnc(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, ushort b);

        /* end of downloading DNC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dncend")]
        public static extern short cnc_dncend(ushort FlibHndl);

        /* start downloading DNC program 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dncstart2")]
        public static extern short cnc_dncstart2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* download DNC program 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dnc2")]
        public static extern short cnc_dnc2(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of downloading DNC program 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dncend2")]
        public static extern short cnc_dncend2(ushort FlibHndl, short a);

        /* read the diagnosis data of DNC operation */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddncdgndt")]
        public static extern short cnc_rddncdgndt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDNCDGN a);

        /* start uploading NC program */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upstart")]
        public static extern short cnc_upstart(ushort FlibHndl, short a);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_upstarto8")]
    public static extern short cnc_upstart( ushort FlibHndl, int a );
#endif

        /* upload NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upload")]
        public static extern short cnc_upload(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBUP a, ref ushort b);

        /* upload NC program(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cupload")]
        public static extern short cnc_cupload(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBUP a, ref ushort b);

        /* end of uploading NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upend")]
        public static extern short cnc_upend(ushort FlibHndl);

        /* start uploading NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upstart3")]
        public static extern short cnc_upstart3(ushort FlibHndl, short a, int b, int c);

        /* start uploading NC program special 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upstart3_f")]
        public static extern short cnc_upstart3_f(ushort FlibHndl,
            short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, [In, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* upload NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upload3")]
        public static extern short cnc_upload3(ushort FlibHndl, ref int a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of uploading NC program 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upend3")]
        public static extern short cnc_upend3(ushort FlibHndl);

        /* start uploading NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upstart4")]
        public static extern short cnc_upstart4(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* upload NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upload4")]
        public static extern short cnc_upload4(ushort FlibHndl, ref int a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* end of uploading NC program 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_upend4")]
        public static extern short cnc_upend4(ushort FlibHndl);

        /* read buffer status for downloading/verification NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_buff")]
        public static extern short cnc_buff(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBBUF a);

        /* search specified program */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_search")]
        public static extern short cnc_search(ushort FlibHndl, short a);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_searcho8")]
    public static extern short cnc_search( ushort FlibHndl, int a );
#endif

        /* search specified program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_search2")]
        public static extern short cnc_search2(ushort FlibHndl, int a);

        /* delete all programs */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_delall")]
        public static extern short cnc_delall(ushort FlibHndl);

        /* delete specified program */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_delete")]
        public static extern short cnc_delete(ushort FlibHndl, short a);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_deleteo8")]
    public static extern short cnc_delete( ushort FlibHndl, int a );
#endif

        /* delete program (area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_delrange")]
        public static extern short cnc_delrange(ushort FlibHndl, int a, int b);

        /* read program directory */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogdir")]
        public static extern short cnc_rdprogdir(ushort FlibHndl,
            short a, short b, short c, ushort d, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGDIR e);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rdprogdiro8")]
    public static extern short cnc_rdprogdir( ushort FlibHndl, 
        short a, short b, short c, ushort d, [Out,MarshalAs(UnmanagedType.LPStruct)] PRGDIR e );
#endif

        /* read program information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdproginfo")]
        public static extern short cnc_rdproginfo(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBNC_1 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdproginfo")]
        public static extern short cnc_rdproginfo(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBNC_2 c);

        /* read program number under execution */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprgnum")]
        public static extern short cnc_rdprgnum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPRO a);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rdprgnumo8")]
    public static extern short cnc_rdprgnum( ushort FlibHndl, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBPRO a );
#endif

        /* read program name under execution */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_exeprgname")]
        public static extern short cnc_exeprgname(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBEXEPRG a);

        /* read sequence number under execution */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdseqnum")]
        public static extern short cnc_rdseqnum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSEQ a);

        /* search specified sequence number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_seqsrch")]
        public static extern short cnc_seqsrch(ushort FlibHndl, int a);

        /* search specified sequence number (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_seqsrch2")]
        public static extern short cnc_seqsrch2(ushort FlibHndl, int a);

        /* rewind cursor of NC program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rewind")]
        public static extern short cnc_rewind(ushort FlibHndl);

        /* read block counter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdblkcount")]
        public static extern short cnc_rdblkcount(ushort FlibHndl, out int a);

        /* read program under execution */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdexecprog")]
        public static extern short cnc_rdexecprog(ushort FlibHndl, ref ushort a, out short b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* read program for MDI operation */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmdiprog")]
        public static extern short cnc_rdmdiprog(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write program for MDI operation */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmdiprog")]
        public static extern short cnc_wrmdiprog(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read execution pointer for MDI operation */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmdipntr")]
        public static extern short cnc_rdmdipntr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDIP a);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rdmdipntro8")]
    public static extern short cnc_rdmdipntr( ushort FlibHndl, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBMDIP a );
#endif

        /* write execution pointer for MDI operation */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmdipntr")]
        public static extern short cnc_wrmdipntr(ushort FlibHndl, int a);

        /* register new program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_newprog")]
        public static extern short cnc_newprog(ushort FlibHndl, int a);

        /* copy program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_copyprog")]
        public static extern short cnc_copyprog(ushort FlibHndl, int a, int b);

        /* rename program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_renameprog")]
        public static extern short cnc_renameprog(ushort FlibHndl, int a, int b);

        /* condense program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_condense")]
        public static extern short cnc_condense(ushort FlibHndl, short a, int b);

        /* merge program */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_mergeprog")]
        public static extern short cnc_mergeprog(ushort FlibHndl, short a, int b, uint c, int d);

        /* read current program and its pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdactpt")]
        public static extern short cnc_rdactpt(ushort FlibHndl, out int a, out int b);

        /* read current program and its pointer and UV macro pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rduvactpt")]
        public static extern short cnc_rduvactpt(ushort FlibHndl, out int a, out int b, out int c);

        /* set current program and its pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wractpt")]
        public static extern short cnc_wractpt(ushort FlibHndl, int a, short b, ref int c);

        /* line edit (read program) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogline")]
        public static extern short cnc_rdprogline(ushort FlibHndl,
            int a, uint b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c, ref uint d, ref uint e);

        /* line edit (read program) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogline2")]
        public static extern short cnc_rdprogline2(ushort FlibHndl,
            int a, uint b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c, ref uint d, ref uint e);

        /* line edit (write program) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrprogline")]
        public static extern short cnc_wrprogline(ushort FlibHndl, int a, uint b, [In, MarshalAs(UnmanagedType.AsAny)] Object c, uint d);

        /* line edit (delete line in program) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_delprogline")]
        public static extern short cnc_delprogline(ushort FlibHndl, int a, uint b, uint c);

        /* line edit (search string) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_searchword")]
        public static extern short cnc_searchword(ushort FlibHndl,
            int a, uint b, short c, short d, uint e, [In, MarshalAs(UnmanagedType.AsAny)] Object f);

        /* line edit (search string) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_searchresult")]
        public static extern short cnc_searchresult(ushort FlibHndl, out uint a);

        /* line edit (read program by file name) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_line")]
        public static extern short cnc_rdpdf_line(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, uint b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c, ref uint d, ref uint e);

        /* program lock */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setpglock")]
        public static extern short cnc_setpglock(ushort FlibHndl, int a);

        /* program unlock */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_resetpglock")]
        public static extern short cnc_resetpglock(ushort FlibHndl, int a);

        /* read the status of the program lock */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpglockstat")]
        public static extern short cnc_rdpglockstat(ushort FlibHndl, out int a, out int b);

        /* create file or directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_add")]
        public static extern short cnc_pdf_add(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* condense program file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_cond")]
        public static extern short cnc_pdf_cond(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* change attribute of program file and directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpdf_attr")]
        public static extern short cnc_wrpdf_attr(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [Out, MarshalAs(UnmanagedType.LPStruct)] IDBPDFTDIR b);

        /* copy program file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_copy")]
        public static extern short cnc_pdf_copy(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* delete file or directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_del")]
        public static extern short cnc_pdf_del(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* line edit (write program by file name) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpdf_line")]
        public static extern short cnc_wrpdf_line(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, uint b, [In, MarshalAs(UnmanagedType.AsAny)] Object c, uint d);

        /* line edit (delete line by file name) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_delline")]
        public static extern short cnc_pdf_delline(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, uint b, uint c);

        /* move program file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_move")]
        public static extern short cnc_pdf_move(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read current program and its pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_rdactpt")]
        public static extern short cnc_pdf_rdactpt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a, out int b);

        /* read selected file name */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_rdmain")]
        public static extern short cnc_pdf_rdmain(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* rename file or directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_rename")]
        public static extern short cnc_pdf_rename(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* line edit (search string) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_searchword")]
        public static extern short cnc_pdf_searchword(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, uint b, uint c, uint d, uint e, [In, MarshalAs(UnmanagedType.AsAny)] Object f);

        /* line edit (search string) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_searchresult")]
        public static extern short cnc_pdf_searchresult(ushort FlibHndl, out uint a);

        /* select program file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_slctmain")]
        public static extern short cnc_pdf_slctmain(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* set current program and its pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_pdf_wractpt")]
        public static extern short cnc_pdf_wractpt(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b, ref int c);

        /* read program drive information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_inf")]
        public static extern short cnc_rdpdf_inf(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* read program drive directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_drive")]
        public static extern short cnc_rdpdf_drive(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read current directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_curdir")]
        public static extern short cnc_rdpdf_curdir(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* set current directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpdf_curdir")]
        public static extern short cnc_wrpdf_curdir(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read directory (sub directories) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_subdir")]
        public static extern short cnc_rdpdf_subdir(ushort FlibHndl,
            ref short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBPDFSDIR b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPDFSDIR c);

        /* read directory (all files) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_alldir")]
        public static extern short cnc_rdpdf_alldir(ushort FlibHndl, ref short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* read file count in directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpdf_subdirn")]
        public static extern short cnc_rdpdf_subdirn(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPDFNFIL b);

        /*---------------------------*/
        /* CNC: NC file data related */
        /*---------------------------*/

        /* read tool offset value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofs")]
        public static extern short cnc_rdtofs(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTOFS d);

        /* write tool offset value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofs")]
        public static extern short cnc_wrtofs(ushort FlibHndl, short a, short b, short c, int d);

        /* read tool offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsr")]
        public static extern short cnc_rdtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_1 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsr")]
        public static extern short cnc_rdtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_2 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsr")]
        public static extern short cnc_rdtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_3 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsr")]
        public static extern short cnc_rdtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTO_2 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsr")]
        public static extern short cnc_rdtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTO_3 e);

        /* write tool offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofsr")]
        public static extern short cnc_wrtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_1 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofsr")]
        public static extern short cnc_wrtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_2 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofsr")]
        public static extern short cnc_wrtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTO_1_3 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofsr")]
        public static extern short cnc_wrtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTO_2 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtofsr")]
        public static extern short cnc_wrtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTO_3 b);

        /* read work zero offset value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdzofs")]
        public static extern short cnc_rdzofs(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBZOFS d);

        /* write work zero offset value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrzofs")]
        public static extern short cnc_wrzofs(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBZOFS b);

        /* read work zero offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdzofsr")]
        public static extern short cnc_rdzofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBZOR e);

        /* write work zero offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrzofsr")]
        public static extern short cnc_wrzofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBZOR b);

        /* read mesured point value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmsptype")]
        public static extern short cnc_rdmsptype(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBMSTP d);

        /* write mesured point value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmsptype")]
        public static extern short cnc_wrmsptype(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBMSTP d);

        /* read parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam")]
        public static extern short cnc_rdparam(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_1 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam")]
        public static extern short cnc_rdparam(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_2 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam")]
        public static extern short cnc_rdparam(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_3 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam")]
        public static extern short cnc_rdparam(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_4 d);

        /* write parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrparam")]
        public static extern short cnc_wrparam(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_1 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrparam")]
        public static extern short cnc_wrparam(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_2 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrparam")]
        public static extern short cnc_wrparam(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_3 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrparam")]
        public static extern short cnc_wrparam(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_4 b);

        /* read parameter(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparar")]
        public static extern short cnc_rdparar(ushort FlibHndl,
            ref short a, short b, ref short c, ref short d, [Out, MarshalAs(UnmanagedType.AsAny)] Object e);
        //  [DllImport("FWLIB32.dll", EntryPoint="cnc_rdparar")]
        //  public static extern short cnc_rdparar( ushort FlibHndl,
        //      ref short a, short b, ref short c, ref short d, [Out,MarshalAs(UnmanagedType.LPStruct)] IODBPSD_A e );
        //  [DllImport("FWLIB32.dll", EntryPoint="cnc_rdparar")]
        //  public static extern short cnc_rdparar( ushort FlibHndl,
        //      ref short a, short b, ref short c, ref short d, [Out,MarshalAs(UnmanagedType.LPStruct)] IODBPSD_B e );
        //  [DllImport("FWLIB32.dll", EntryPoint="cnc_rdparar")]
        //  public static extern short cnc_rdparar( ushort FlibHndl,
        //      ref short a, short b, ref short c, ref short d, [Out,MarshalAs(UnmanagedType.LPStruct)] IODBPSD_C e );
        //  [DllImport("FWLIB32.dll", EntryPoint="cnc_rdparar")]
        //  public static extern short cnc_rdparar( ushort FlibHndl,
        //      ref short a, short b, ref short c, ref short d, [Out,MarshalAs(UnmanagedType.LPStruct)] IODBPSD_D e );

        /* write parameter(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrparas")]
        public static extern short cnc_wrparas(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdset")]
        public static extern short cnc_rdset(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_1 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdset")]
        public static extern short cnc_rdset(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_2 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdset")]
        public static extern short cnc_rdset(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_3 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdset")]
        public static extern short cnc_rdset(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_4 d);

        /* write setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrset")]
        public static extern short cnc_wrset(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_1 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrset")]
        public static extern short cnc_wrset(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_2 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrset")]
        public static extern short cnc_wrset(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_3 b);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrset")]
        public static extern short cnc_wrset(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_4 b);

        /* read setting data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsetr")]
        public static extern short cnc_rdsetr(ushort FlibHndl,
            ref short a, short b, ref short c, ref short d, [Out, MarshalAs(UnmanagedType.AsAny)] Object e);

        /* write setting data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrsets")]
        public static extern short cnc_wrsets(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read parameters */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam_ext")]
        public static extern short cnc_rdparam_ext(ushort FlibHndl,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] IODBPRMNO a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPRM c);

        /* read parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam3")]
        public static extern short cnc_rdparam3(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_1 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam3")]
        public static extern short cnc_rdparam3(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_2 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam3")]
        public static extern short cnc_rdparam3(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_3 e);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparam3")]
        public static extern short cnc_rdparam3(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSD_4 e);

        /* async parameter write start */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_start_async_wrparam")]
        public static extern short cnc_start_async_wrparam(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPRM a);

        /* async parameter write end */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_end_async_wrparam")]
        public static extern short cnc_end_async_wrparam(ushort FlibHndl, out short a);

        /* read cause of busy for async parameter write */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_async_busy_state")]
        public static extern short cnc_async_busy_state(ushort FlibHndl, out short a);

        /* read diagnosis data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddiag_ext")]
        public static extern short cnc_rddiag_ext(ushort FlibHndl,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] IODBPRMNO a, short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPRM c);

        /* read pitch error compensation data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpitchr")]
        public static extern short cnc_rdpitchr(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPI d);

        /* write pitch error compensation data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpitchr")]
        public static extern short cnc_wrpitchr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPI b);

        /* read custom macro variable */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmacro")]
        public static extern short cnc_rdmacro(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBM c);

        /* write custom macro variable */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmacro")]
        public static extern short cnc_wrmacro(ushort FlibHndl, short a, short b, int c, short d);

        /* read custom macro variables(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmacror")]
        public static extern short cnc_rdmacror(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBMR d);

        /* write custom macro variables(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmacror")]
        public static extern short cnc_wrmacror(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBMR b);

        /* read custom macro variables(IEEE double version) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmacror2")]
        public static extern short cnc_rdmacror2(ushort FlibHndl, int a, ref int b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* write custom macro variables(IEEE double version) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmacror2")]
        public static extern short cnc_wrmacror2(ushort FlibHndl, int a, ref int b, [In, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* read P code macro variable */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpmacro")]
        public static extern short cnc_rdpmacro(ushort FlibHndl, int a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPM b);

        /* write P code macro variable */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpmacro")]
        public static extern short cnc_wrpmacro(ushort FlibHndl, int a, int b, short c);

        /* read P code macro variables(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpmacror")]
        public static extern short cnc_rdpmacror(ushort FlibHndl,
            int a, int b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPR d);

        /* write P code macro variables(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpmacror")]
        public static extern short cnc_wrpmacror(ushort FlibHndl, ushort a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPR b);

        /* read P code macro variables(IEEE double version) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpmacror2")]
        public static extern short cnc_rdpmacror2(ushort FlibHndl, uint a, ref uint b, ushort c, [Out, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* write P code macro variables(IEEE double version) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpmacror2")]
        public static extern short cnc_wrpmacror2(ushort FlibHndl, uint a, ref uint b, ushort c, [In, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* read tool offset information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsinfo")]
        public static extern short cnc_rdtofsinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLINF a);

        /* read tool offset information(2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtofsinfo2")]
        public static extern short cnc_rdtofsinfo2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLINF2 a);

        /* read work zero offset information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdzofsinfo")]
        public static extern short cnc_rdzofsinfo(ushort FlibHndl, out short a);

        /* read pitch error compensation data information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpitchinfo")]
        public static extern short cnc_rdpitchinfo(ushort FlibHndl, out short a);

        /* read custom macro variable information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmacroinfo")]
        public static extern short cnc_rdmacroinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMVINF a);

        /* read P code macro variable information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpmacroinfo")]
        public static extern short cnc_rdpmacroinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMINF a);

        /* read validity of tool offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_tofs_rnge")]
        public static extern short cnc_tofs_rnge(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDATRNG c);

        /* read validity of work zero offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_zofs_rnge")]
        public static extern short cnc_zofs_rnge(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDATRNG c);

        /* read validity of work zero offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wksft_rnge")]
        public static extern short cnc_wksft_rnge(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDATRNG b);

        /* read the information for function cnc_rdhsparam() */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhsprminfo")]
        public static extern short cnc_rdhsprminfo(ushort FlibHndl, int a, [Out, MarshalAs(UnmanagedType.LPStruct)] HSPINFO_data b);

        /* read parameters at the high speed */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhsparam")]
        public static extern short cnc_rdhsparam(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.LPStruct)] HSPINFO b, [Out, MarshalAs(UnmanagedType.LPStruct)] HSPDATA_1 c);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhsparam")]
        public static extern short cnc_rdhsparam(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.LPStruct)] HSPINFO b, [Out, MarshalAs(UnmanagedType.LPStruct)] HSPDATA_2 c);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhsparam")]
        public static extern short cnc_rdhsparam(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.LPStruct)] HSPINFO b, [Out, MarshalAs(UnmanagedType.LPStruct)] HSPDATA_3 c);

        /*----------------------------------------*/
        /* CNC: Tool life management data related */
        /*----------------------------------------*/

        /* read tool life management data(tool group number) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpid")]
        public static extern short cnc_rdgrpid(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE1 b);

        /* read tool life management data(number of tool groups) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdngrp")]
        public static extern short cnc_rdngrp(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE2 a);

        /* read tool life management data(number of tools) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdntool")]
        public static extern short cnc_rdntool(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE3 b);

        /* read tool life management data(tool life) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlife")]
        public static extern short cnc_rdlife(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE3 b);

        /* read tool life management data(tool lift counter) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcount")]
        public static extern short cnc_rdcount(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE3 b);

        /* read tool life management data(tool length number-1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd1length")]
        public static extern short cnc_rd1length(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(tool length number-2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd2length")]
        public static extern short cnc_rd2length(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(cutter compensation no.-1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd1radius")]
        public static extern short cnc_rd1radius(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(cutter compensation no.-2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd2radius")]
        public static extern short cnc_rd2radius(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(tool information-1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_t1info")]
        public static extern short cnc_t1info(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(tool information-2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_t2info")]
        public static extern short cnc_t2info(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(tool number) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_toolnum")]
        public static extern short cnc_toolnum(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE4 c);

        /* read tool life management data(tool number, tool life, tool life counter)(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtoolrng")]
        public static extern short cnc_rdtoolrng(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTR d);

        /* read tool life management data(all data within group) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtoolgrp")]
        public static extern short cnc_rdtoolgrp(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTG c);

        /* write tool life management data(tool life counter) (area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrcountr")]
        public static extern short cnc_wrcountr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBWRC b);

        /* read tool life management data(used tool group number) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdusegrpid")]
        public static extern short cnc_rdusegrpid(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBUSEGR a);

        /* read tool life management data(max. number of tool groups) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmaxgrp")]
        public static extern short cnc_rdmaxgrp(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLFNO a);

        /* read tool life management data(maximum number of tool within group) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmaxtool")]
        public static extern short cnc_rdmaxtool(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLFNO a);

        /* read tool life management data(used tool no. within group) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdusetlno")]
        public static extern short cnc_rdusetlno(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLUSE d);

        /* read tool life management data(tool data1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd1tlifedata")]
        public static extern short cnc_rd1tlifedata(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTD c);

        /* read tool life management data(tool data2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd2tlifedata")]
        public static extern short cnc_rd2tlifedata(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTD c);

        /* write tool life management data(tool data1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wr1tlifedata")]
        public static extern short cnc_wr1tlifedata(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTD a);

        /* write tool life management data(tool data2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wr2tlifedata")]
        public static extern short cnc_wr2tlifedata(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTD a);

        /* read tool life management data(tool group information) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpinfo")]
        public static extern short cnc_rdgrpinfo(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTGI d);

        /* read tool life management data(tool group information 2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpinfo2")]
        public static extern short cnc_rdgrpinfo2(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTGI2 d);

        /* read tool life management data(tool group information 3) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpinfo3")]
        public static extern short cnc_rdgrpinfo3(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTGI3 d);

        /* read tool life management data(tool group information 4) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpinfo4")]
        public static extern short cnc_rdgrpinfo4(ushort FlibHndl,
            short a, short b, short c, out short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTGI4 e);

        /* write tool life management data(tool group information) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrgrpinfo")]
        public static extern short cnc_wrgrpinfo(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTGI b);

        /* write tool life management data(tool group information 2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrgrpinfo2")]
        public static extern short cnc_wrgrpinfo2(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTGI2 b);

        /* write tool life management data(tool group information 3) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrgrpinfo3")]
        public static extern short cnc_wrgrpinfo3(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTGI3 b);

        /* delete tool life management data(tool group) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_deltlifegrp")]
        public static extern short cnc_deltlifegrp(ushort FlibHndl, short a);

        /* insert tool life management data(tool data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_instlifedt")]
        public static extern short cnc_instlifedt(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IDBITD a);

        /* delete tool life management data(tool data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_deltlifedt")]
        public static extern short cnc_deltlifedt(ushort FlibHndl, short a, short b);

        /* clear tool life management data(tool life counter, tool information)(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clrcntinfo")]
        public static extern short cnc_clrcntinfo(ushort FlibHndl, short a, short b);

        /* read tool life management data(tool group number) 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrpid2")]
        public static extern short cnc_rdgrpid2(ushort FlibHndl, int a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLIFE5 b);

        /* read tool life management data(tool data1) 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd1tlifedat2")]
        public static extern short cnc_rd1tlifedat2(ushort FlibHndl,
            short a, int b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTD2 c);

        /* write tool life management data(tool data1) 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wr1tlifedat2")]
        public static extern short cnc_wr1tlifedat2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTD2 a);

        /* read tool life management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtlinfo")]
        public static extern short cnc_rdtlinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBTLINFO a);

        /* read tool life management data(used tool group number) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtlusegrp")]
        public static extern short cnc_rdtlusegrp(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBUSEGRP a);

        /* read tool life management data(tool group information 2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtlgrp")]
        public static extern short cnc_rdtlgrp(ushort FlibHndl,
            int a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLGRP c);

        /* read tool life management data (tool data1) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtltool")]
        public static extern short cnc_rdtltool(ushort FlibHndl,
            int a, int b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLTOOL d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdexchgtgrp")]
        public static extern short cnc_rdexchgtgrp(ushort FlibHndl,
            ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBEXGP c);

        /*-----------------------------------*/
        /* CNC: Tool management data related */
        /*-----------------------------------*/

        /* new registration of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_regtool")]
        public static extern short cnc_regtool(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG c);

        /* new registration of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_regtool_f2")]
        public static extern short cnc_regtool_f2(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG_F2 c);

        /* deletion of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_deltool")]
        public static extern short cnc_deltool(ushort FlibHndl, short a, ref short b);

        /* lead of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtool")]
        public static extern short cnc_rdtool(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG c);

        /* lead of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtool_f2")]
        public static extern short cnc_rdtool_f2(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG_F2 c);

        /* write of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtool")]
        public static extern short cnc_wrtool(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG b);

        /* write of individual data of tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtool2")]
        public static extern short cnc_wrtool2(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBTLM b);

        /* write tool management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtool_f2")]
        public static extern short cnc_wrtool_f2(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMNG_F2_data b);

        /* new registration of magazine management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_regmagazine")]
        public static extern short cnc_regmagazine(ushort FlibHndl, ref short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMAG b);

        /* deletion of magazine management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_delmagazine")]
        public static extern short cnc_delmagazine(ushort FlibHndl, ref short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLMAG2 b);

        /* lead of magazine management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmagazine")]
        public static extern short cnc_rdmagazine(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLMAG b);

        /* Individual write of magazine management data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmagazine")]
        public static extern short cnc_wrmagazine(ushort FlibHndl, short a, short b, short c);


        /*-------------------------------------*/
        /* CNC: Operation history data related */
        /*-------------------------------------*/

        /* stop logging operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopophis")]
        public static extern short cnc_stopophis(ushort FlibHndl);

        /* restart logging operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startophis")]
        public static extern short cnc_startophis(ushort FlibHndl);

        /* read number of operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophisno")]
        public static extern short cnc_rdophisno(ushort FlibHndl, out ushort a);

        /* read operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry")]
        public static extern short cnc_rdophistry(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHIS d);

        /* read operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry2")]
        public static extern short cnc_rdophistry2(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* read operation history data F30i*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_1 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_2 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_3 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_4 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_5 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_6 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_7 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_8 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_9 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_10 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdophistry4")]
        public static extern short cnc_rdophistry4(ushort FlibHndl,
            ushort a, ref ushort b, ref ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOPHIS4_11 d);

        /* read number of alarm history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhisno")]
        public static extern short cnc_rdalmhisno(ushort FlibHndl, out ushort a);

        /* read alarm history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhistry")]
        public static extern short cnc_rdalmhistry(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAHIS d);

        /* read alarm history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhistry_w")]
        public static extern short cnc_rdalmhistry_w(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAHIS d);

        /* read alarm history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhistry2")]
        public static extern short cnc_rdalmhistry2(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAHIS2 d);

        /* read alarm history data F30i*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhistry3")]
        public static extern short cnc_rdalmhistry3(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAHIS3 d);

        /* read alarm history data F30i*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmhistry5")]
        public static extern short cnc_rdalmhistry5(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAHIS5 d);

        /* clear operation history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clearophis")]
        public static extern short cnc_clearophis(ushort FlibHndl, short a);

        /* read signals related operation history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhissgnl")]
        public static extern short cnc_rdhissgnl(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSIG a);

        /* read signals related operation history 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhissgnl2")]
        public static extern short cnc_rdhissgnl2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSIG2 a);

        /* read signals related operation history 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhissgnl3")]
        public static extern short cnc_rdhissgnl3(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSIG3 a);

        /* write signals related operation history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhissgnl")]
        public static extern short cnc_wrhissgnl(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSIG a);

        /* write signals related operation history 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhissgnl2")]
        public static extern short cnc_wrhissgnl2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSIG2 a);

        /* write signals related operation history for F30i*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhissgnl3")]
        public static extern short cnc_wrhissgnl3(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSIG3 a);

        /* read number of operater message history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdomhisno")]
        public static extern short cnc_rdomhisno(ushort FlibHndl, out ushort a);

        /*-------------*/
        /* CNC: Others */
        /*-------------*/

        /* read CNC system information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sysinfo")]
        public static extern short cnc_sysinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYS a);

        /* read CNC status information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_statinfo")]
        public static extern short cnc_statinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBST a);

        /* read alarm status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_alarm")]
        public static extern short cnc_alarm(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBALM a);

        /* read alarm status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_alarm2")]
        public static extern short cnc_alarm2(ushort FlibHndl, out int a);

        /* read alarm information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalminfo")]
        public static extern short cnc_rdalminfo(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ALMINFO_1 d);

        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalminfo")]
        public static extern short cnc_rdalminfo(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ALMINFO_2 d);

        /* read alarm message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmmsg")]
        public static extern short cnc_rdalmmsg(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBALMMSG c);

        /* read alarm message(2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdalmmsg2")]
        public static extern short cnc_rdalmmsg2(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBALMMSG2 c);

        /* clear CNC alarm */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clralm")]
        public static extern short cnc_clralm(ushort FlibHndl, short a);

        /* read modal data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_modal")]
        public static extern short cnc_modal(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDL_1 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_modal")]
        public static extern short cnc_modal(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDL_2 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_modal")]
        public static extern short cnc_modal(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDL_3 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_modal")]
        public static extern short cnc_modal(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDL_4 c);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_modal")]
        public static extern short cnc_modal(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDL_5 c);

        /* read G code */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgcode")]
        public static extern short cnc_rdgcode(ushort FlibHndl,
            short a, short b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBGCD d);

        /* read command value */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcommand")]
        public static extern short cnc_rdcommand(ushort FlibHndl,
            short a, short b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBCMD d);

        /* read diagnosis data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
        public static extern short cnc_diagnoss(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_1 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
        public static extern short cnc_diagnoss(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_2 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
        public static extern short cnc_diagnoss(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_3 d);
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
        public static extern short cnc_diagnoss(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_4 d);

        /* read diagnosis data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnosr")]
        public static extern short cnc_diagnosr(ushort FlibHndl,
            ref short a, short b, ref short c, ref short d, [Out, MarshalAs(UnmanagedType.AsAny)] Object e);
        //  [DllImport("FWLIB32.dll", EntryPoint="cnc_diagnosr")]
        //  public static extern short cnc_diagnosr( ushort FlibHndl,
        //      ref short a, short b, ref short c, ref short d, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBDGN e );

        /* read A/D conversion data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_adcnv")]
        public static extern short cnc_adcnv(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAD c);

        /* read operator's message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopmsg")]
        public static extern short cnc_rdopmsg(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] OPMSG c);

        /* read operator's message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopmsg2")]
        public static extern short cnc_rdopmsg2(ushort FlibHndl, short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] OPMSG2 c);

        /* read operator's message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopmsg3")]
        public static extern short cnc_rdopmsg3(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] OPMSG3 c);

        /* set path number(for 4 axes lathes, multi-path) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setpath")]
        public static extern short cnc_setpath(ushort FlibHndl, short a);

        /* get path number(for 4 axes lathes, multi-path) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getpath")]
        public static extern short cnc_getpath(ushort FlibHndl, out short a, out short b);

        /* allocate library handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allclibhndl")]
        public static extern short cnc_allclibhndl(out ushort FlibHndl);

        /* free library handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_freelibhndl")]
        public static extern short cnc_freelibhndl(ushort FlibHndl);

        /* get library option */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getlibopt")]
        public static extern short cnc_getlibopt(ushort FlibHndl, int a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* set library option */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setlibopt")]
        public static extern short cnc_setlibopt(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, int c);

        /* get custom macro type */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getmactype")]
        public static extern short cnc_getmactype(ushort FlibHndl, out short a);

        /* set custom macro type */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setmactype")]
        public static extern short cnc_setmactype(ushort FlibHndl, short a);

        /* get P code macro type */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getpmactype")]
        public static extern short cnc_getpmactype(ushort FlibHndl, out short a);

        /* set P code macro type */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setpmactype")]
        public static extern short cnc_setpmactype(ushort FlibHndl, short a);

        /* get screen status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getcrntscrn")]
        public static extern short cnc_getcrntscrn(ushort FlibHndl, out short a);

        /* change screen mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_slctscrn")]
        public static extern short cnc_slctscrn(ushort FlibHndl, short a);

        /* read CNC configuration information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sysconfig")]
        public static extern short cnc_sysconfig(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSC a);

        /* read program restart information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprstrinfo")]
        public static extern short cnc_rdprstrinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPRS a);

        /* search sequence number for program restart */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rstrseqsrch")]
        public static extern short cnc_rstrseqsrch(ushort FlibHndl, int a, int b, short c, short d);

        /* search sequence number for program restart 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rstrseqsrch2")]
        public static extern short cnc_rstrseqsrch2(ushort FlibHndl, int a, int b, short c, short d, int e);

        /* read output signal image of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopnlsgnl")]
        public static extern short cnc_rdopnlsgnl(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSGNL b);

        /* write output signal of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wropnlsgnl")]
        public static extern short cnc_wropnlsgnl(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSGNL a);

        /* read general signal image of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopnlgnrl")]
        public static extern short cnc_rdopnlgnrl(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBGNRL b);

        /* write general signal image of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wropnlgnrl")]
        public static extern short cnc_wropnlgnrl(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBGNRL a);

        /* read general signal name of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopnlgsname")]
        public static extern short cnc_rdopnlgsname(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBRDNA b);

        /* write general signal name of software operator's panel  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wropnlgsname")]
        public static extern short cnc_wropnlgsname(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBRDNA a);

        /* get detail error */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getdtailerr")]
        public static extern short cnc_getdtailerr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBERR a);

        /* read informations of CNC parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparainfo")]
        public static extern short cnc_rdparainfo(ushort FlibHndl,
            short a, ushort b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPARAIF c);

        /* read informations of CNC setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsetinfo")]
        public static extern short cnc_rdsetinfo(ushort FlibHndl,
            short a, ushort b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSETIF c);

        /* read informations of CNC diagnose data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddiaginfo")]
        public static extern short cnc_rddiaginfo(ushort FlibHndl,
            short a, ushort b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDIAGIF c);

        /* read maximum, minimum and total number of CNC parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdparanum")]
        public static extern short cnc_rdparanum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPARANUM a);

        /* read maximum, minimum and total number of CNC setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsetnum")]
        public static extern short cnc_rdsetnum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSETNUM a);

        /* read maximum, minimum and total number of CNC diagnose data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddiagnum")]
        public static extern short cnc_rddiagnum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDIAGNUM a);

        /* get maximum valid figures and number of decimal places */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getfigure")]
        public static extern short cnc_getfigure(ushort FlibHndl,
            short a, out short b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c, [Out, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* read F-ROM information on CNC  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdfrominfo")]
        public static extern short cnc_rdfrominfo(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBFINFO b);

        /* start of reading F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromsvstart")]
        public static extern short cnc_fromsvstart(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, int c);

        /* read F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromsave")]
        public static extern short cnc_fromsave(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* end of reading F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromsvend")]
        public static extern short cnc_fromsvend(ushort FlibHndl);

        /* start of writing F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromldstart")]
        public static extern short cnc_fromldstart(ushort FlibHndl, short a, int b);

        /* write F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromload")]
        public static extern short cnc_fromload(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, ref int b);

        /* end of writing F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromldend")]
        public static extern short cnc_fromldend(ushort FlibHndl);

        /* delete F-ROM data on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromdelete")]
        public static extern short cnc_fromdelete(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, int c);

        /* read S-RAM information on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsraminfo")]
        public static extern short cnc_rdsraminfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSINFO a);

        /* start of reading S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srambkstart")]
        public static extern short cnc_srambkstart(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, int b);

        /* read S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srambackup")]
        public static extern short cnc_srambackup(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* end of reading S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srambkend")]
        public static extern short cnc_srambkend(ushort FlibHndl);

        /* read F-ROM information on CNC  */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getfrominfo")]
        public static extern short cnc_getfrominfo(ushort FlibHndl,
            short a, out short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBFINFORM c);

        /* start of reading F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromgetstart")]
        public static extern short cnc_fromgetstart(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromget")]
        public static extern short cnc_fromget(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* end of reading F-ROM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromgetend")]
        public static extern short cnc_fromgetend(ushort FlibHndl);

        /* start of writing F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromputstart")]
        public static extern short cnc_fromputstart(ushort FlibHndl, short a);

        /* write F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromput")]
        public static extern short cnc_fromput(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, ref int b);

        /* end of writing F-ROM data to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromputend")]
        public static extern short cnc_fromputend(ushort FlibHndl);

        /* delete F-ROM data on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_fromremove")]
        public static extern short cnc_fromremove(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read S-RAM information on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getsraminfo")]
        public static extern short cnc_getsraminfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSINFO a);

        /* start of reading S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramgetstart")]
        public static extern short cnc_sramgetstart(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* start of reading S-RAM data from CNC (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramgetstart2")]
        public static extern short cnc_sramgetstart2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramget")]
        public static extern short cnc_sramget(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* read S-RAM data from CNC (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramget2")]
        public static extern short cnc_sramget2(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* end of reading S-RAM data from CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramgetend")]
        public static extern short cnc_sramgetend(ushort FlibHndl);

        /* end of reading S-RAM data from CNC (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sramgetend2")]
        public static extern short cnc_sramgetend2(ushort FlibHndl);

        /* read number of S-RAM data kind on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsramnum")]
        public static extern short cnc_rdsramnum(ushort FlibHndl, out short a);

        /* read S-RAM data address information on CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsramaddr")]
        public static extern short cnc_rdsramaddr(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.LPStruct)] SRAMADDR b);

        /* get current NC data protection information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getlockstat")]
        public static extern short cnc_getlockstat(ushort FlibHndl, short a, out byte b);

        /* change NC data protection status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_chgprotbit")]
        public static extern short cnc_chgprotbit(ushort FlibHndl, short a, ref byte b, int c);

        /* transfer a file from host computer to CNC by FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvftpget")]
        public static extern short cnc_dtsvftpget(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* transfer a file from CNC to host computer by FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvftpput")]
        public static extern short cnc_dtsvftpput(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* get transfer status for FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvftpstat")]
        public static extern short cnc_dtsvftpstat(ushort FlibHndl);

        /* read file directory in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvrdpgdir")]
        public static extern short cnc_dtsvrdpgdir(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDSDIR c);

        /* delete files in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvdelete")]
        public static extern short cnc_dtsvdelete(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* down load from CNC (transfer a file from CNC to MMC) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvdownload")]
        public static extern short cnc_dtsvdownload(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* up load to CNC (transfer a file from MMC to CNC) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvupload")]
        public static extern short cnc_dtsvupload(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* close upload/download between Data Server and CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvcnclupdn")]
        public static extern short cnc_dtsvcnclupdn(ushort FlibHndl);

        /* get transfer status for up/down load */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvupdnstat")]
        public static extern short cnc_dtsvupdnstat(ushort FlibHndl);

        /* get file name for DNC operation in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvgetdncpg")]
        public static extern short cnc_dtsvgetdncpg(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* set program number of DNC oparation to CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvsetdncpg")]
        public static extern short cnc_dtsvsetdncpg(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read setting data for Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvrdset")]
        public static extern short cnc_dtsvrdset(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBDSSET a);

        /* write setting data for Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvwrset")]
        public static extern short cnc_dtsvwrset(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBDSSET a);

        /* check hard disk in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvchkdsk")]
        public static extern short cnc_dtsvchkdsk(ushort FlibHndl);

        /* format hard disk in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvhdformat")]
        public static extern short cnc_dtsvhdformat(ushort FlibHndl);

        /* save interface area in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvsavecram")]
        public static extern short cnc_dtsvsavecram(ushort FlibHndl);

        /* get interface area in Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvrdcram")]
        public static extern short cnc_dtsvrdcram(ushort FlibHndl, int a, ref int b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* read maintenance information for Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvmntinfo")]
        public static extern short cnc_dtsvmntinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDSMNT a);

        /* get Data Server mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvgetmode")]
        public static extern short cnc_dtsvgetmode(ushort FlibHndl, out short a);

        /* set Data Server mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvsetmode")]
        public static extern short cnc_dtsvsetmode(ushort FlibHndl, short a);

        /* read error message for Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvrderrmsg")]
        public static extern short cnc_dtsvrderrmsg(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* transfar file from Pc to Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvwrfile")]
        public static extern short cnc_dtsvwrfile(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, short c);

        /* transfar file from Data Server to Pc */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dtsvrdfile")]
        public static extern short cnc_dtsvrdfile(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, short c);

        /* read the loop gain for each axis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdloopgain")]
        public static extern short cnc_rdloopgain(ushort FlibHndl, out int a);

        /* read the actual current for each axis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcurrent")]
        public static extern short cnc_rdcurrent(ushort FlibHndl, out short a);

        /* read the actual speed for each axis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsrvspeed")]
        public static extern short cnc_rdsrvspeed(ushort FlibHndl, out int a);

        /* read the operation mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdopmode")]
        public static extern short cnc_rdopmode(ushort FlibHndl, out short a);

        /* read the position deviation S */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdposerrs")]
        public static extern short cnc_rdposerrs(ushort FlibHndl, out int a);

        /* read the position deviation S1 and S2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdposerrs2")]
        public static extern short cnc_rdposerrs2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPSER a);

        /* read the position deviation Z in the rigid tap mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdposerrz")]
        public static extern short cnc_rdposerrz(ushort FlibHndl, out int a);

        /* read the synchronous error in the synchronous control mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsynerrsy")]
        public static extern short cnc_rdsynerrsy(ushort FlibHndl, out int a);

        /* read the synchronous error in the rigid tap mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsynerrrg")]
        public static extern short cnc_rdsynerrrg(ushort FlibHndl, out int a);

        /* read the spindle alarm */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspdlalm")]
        public static extern short cnc_rdspdlalm(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the control input signal */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdctrldi")]
        public static extern short cnc_rdctrldi(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPDI a);

        /* read the control output signal */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdctrldo")]
        public static extern short cnc_rdctrldo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPDO a);

        /* read the number of controled spindle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdnspdl")]
        public static extern short cnc_rdnspdl(ushort FlibHndl, out short a);

        /* read data from FANUC BUS */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdfbusmem")]
        public static extern short cnc_rdfbusmem(ushort FlibHndl,
            short a, short b, int c, int d, [Out, MarshalAs(UnmanagedType.AsAny)] Object e);

        /* write data to FANUC BUS */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrfbusmem")]
        public static extern short cnc_wrfbusmem(ushort FlibHndl,
            short a, short b, int c, int d, [In, MarshalAs(UnmanagedType.AsAny)] Object e);

        /* read the parameter of wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdwaveprm")]
        public static extern short cnc_rdwaveprm(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBWAVE a);

        /* write the parameter of wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrwaveprm")]
        public static extern short cnc_wrwaveprm(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBWAVE a);

        /* read the parameter of wave diagnosis 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdwaveprm2")]
        public static extern short cnc_rdwaveprm2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBWVPRM a);

        /* write the parameter of wave diagnosis 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrwaveprm2")]
        public static extern short cnc_wrwaveprm2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBWVPRM a);

        /* start the sampling for wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wavestart")]
        public static extern short cnc_wavestart(ushort FlibHndl);

        /* stop the sampling for wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wavestop")]
        public static extern short cnc_wavestop(ushort FlibHndl);

        /* read the status of wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wavestat")]
        public static extern short cnc_wavestat(ushort FlibHndl, out short a);

        /* read the data of wave diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdwavedata")]
        public static extern short cnc_rdwavedata(ushort FlibHndl,
            short a, short b, int c, ref int d, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBWVDT e);

        /* read the parameter of wave diagnosis for remort diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrmtwaveprm")]
        public static extern short cnc_rdrmtwaveprm(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBRMTPRM a, short b);

        /* write the parameter of wave diagnosis for remort diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrrmtwaveprm")]
        public static extern short cnc_wrrmtwaveprm(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBRMTPRM a);

        /* start the sampling for wave diagnosis for remort diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rmtwavestart")]
        public static extern short cnc_rmtwavestart(ushort FlibHndl);

        /* stop the sampling for wave diagnosis for remort diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rmtwavestop")]
        public static extern short cnc_rmtwavestop(ushort FlibHndl);

        /* read the status of wave diagnosis for remort diagnosis*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rmtwavestat")]
        public static extern short cnc_rmtwavestat(ushort FlibHndl, out short a);

        /* read the data of wave diagnosis for remort diagnosis */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrmtwavedt")]
        public static extern short cnc_rdrmtwavedt(ushort FlibHndl,
            short a, int b, ref int c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBRMTDT d);

        /* read of address for PMC signal batch save */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsavsigadr")]
        public static extern short cnc_rdsavsigadr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSIGAD a, short b);

        /* write of address for PMC signal batch save */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrsavsigadr")]
        public static extern short cnc_wrsavsigadr(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSIGAD a, out short b);

        /* read of data for PMC signal batch save */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsavsigdata")]
        public static extern short cnc_rdsavsigdata(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c, ref short d);

        /* read M-code group data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmgrpdata")]
        public static extern short cnc_rdmgrpdata(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMGRP c);

        /* write M-code group data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmgrpdata")]
        public static extern short cnc_wrmgrpdata(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IDBMGRP a);

        /* read executing M-code group data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdexecmcode")]
        public static extern short cnc_rdexecmcode(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBEXEM c);

        /* read program restart M-code group data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrstrmcode")]
        public static extern short cnc_rdrstrmcode(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBRSTRM c);

        /* read processing time stamp data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdproctime")]
        public static extern short cnc_rdproctime(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPTIME a);

        /* read MDI program stat */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmdiprgstat")]
        public static extern short cnc_rdmdiprgstat(ushort FlibHndl, out ushort a);

        /* read program directory for processing time data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprgdirtime")]
        public static extern short cnc_rdprgdirtime(ushort FlibHndl,
            ref int a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGDIRTM c);

        /* read program directory 2 */
#if (!ONO8D)
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogdir2")]
        public static extern short cnc_rdprogdir2(ushort FlibHndl,
            short a, ref short b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGDIR2 d);
#else
    [DllImport("FWLIB32.dll", EntryPoint="cnc_rdprogdir2o8")]
    public static extern short cnc_rdprogdir2( ushort FlibHndl,
        short a, ref short b, ref short c, [Out,MarshalAs(UnmanagedType.LPStruct)] PRGDIR2 d );
#endif

        /* read program directory 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogdir3")]
        public static extern short cnc_rdprogdir3(ushort FlibHndl,
            short a, ref int b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGDIR3 d);

        /* read program directory 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdprogdir4")]
        public static extern short cnc_rdprogdir4(ushort FlibHndl,
            short a, int b, ref short c, [Out, MarshalAs(UnmanagedType.LPStruct)] PRGDIR4 d);

        /* read DNC file name for DNC1, DNC2, OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddncfname")]
        public static extern short cnc_rddncfname(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* write DNC file name for DNC1, DNC2, OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrdncfname")]
        public static extern short cnc_wrdncfname(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read communication parameter for DNC1, DNC2, OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcomparam")]
        public static extern short cnc_rdcomparam(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBCPRM a);

        /* write communication parameter for DNC1, DNC2, OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrcomparam")]
        public static extern short cnc_wrcomparam(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBCPRM a);

        /* read log message for DNC2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcomlogmsg")]
        public static extern short cnc_rdcomlogmsg(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read operator message for DNC1, DNC2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcomopemsg")]
        public static extern short cnc_rdcomopemsg(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read recieve message for OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrcvmsg")]
        public static extern short cnc_rdrcvmsg(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read send message for OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsndmsg")]
        public static extern short cnc_rdsndmsg(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* send message for OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sendmessage")]
        public static extern short cnc_sendmessage(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* clear message buffer for OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clrmsgbuff")]
        public static extern short cnc_clrmsgbuff(ushort FlibHndl, short a);

        /* read message recieve status for OSI-Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrcvstat")]
        public static extern short cnc_rdrcvstat(ushort FlibHndl, out ushort a);

        /* read interference check */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdintchk")]
        public static extern short cnc_rdintchk(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBINT e);

        /* write interference check */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrintchk")]
        public static extern short cnc_wrintchk(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBINT b);

        /* read interference check information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdintinfo")]
        public static extern short cnc_rdintinfo(ushort FlibHndl, out short a);

        /* read work coordinate shift */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdwkcdshft")]
        public static extern short cnc_rdwkcdshft(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBWCSF c);

        /* write work coordinate shift */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrwkcdshft")]
        public static extern short cnc_wrwkcdshft(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBWCSF b);

        /* read work coordinate shift measure */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdwkcdsfms")]
        public static extern short cnc_rdwkcdsfms(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBWCSF c);

        /* write work coordinate shift measure */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrwkcdsfms")]
        public static extern short cnc_wrwkcdsfms(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBWCSF b);

        /* stop the sampling for operator message history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopomhis")]
        public static extern short cnc_stopomhis(ushort FlibHndl);

        /* start the sampling for operator message history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startomhis")]
        public static extern short cnc_startomhis(ushort FlibHndl);

        /* read operator message history information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdomhisinfo")]
        public static extern short cnc_rdomhisinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOMIF a);

        /* read operator message history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdomhistry")]
        public static extern short cnc_rdomhistry(ushort FlibHndl,
            ushort a, ref ushort b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOMHIS c);

        /* read operater message history data F30i */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdomhistry2")]
        public static extern short cnc_rdomhistry2(ushort FlibHndl,
            ushort a, ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBOMHIS2 d);

        /* write external key operation history for F30i*/
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrkeyhistry")]
        public static extern short cnc_wrkeyhistry(ushort FlibHndl, byte a);

        /* clear operator message history */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clearomhis")]
        public static extern short cnc_clearomhis(ushort FlibHndl);

        /* read b-axis tool offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdbtofsr")]
        public static extern short cnc_rdbtofsr(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBBTO e);

        /* write b-axis tool offset value(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrbtofsr")]
        public static extern short cnc_wrbtofsr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBBTO b);

        /* read b-axis tool offset information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdbtofsinfo")]
        public static extern short cnc_rdbtofsinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBBTLINF a);

        /* read b-axis command */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdbaxis")]
        public static extern short cnc_rdbaxis(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBBAXIS a);

        /* read CNC system soft series and version */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsyssoft")]
        public static extern short cnc_rdsyssoft(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSS a);

        /* read CNC system soft series and version (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsyssoft2")]
        public static extern short cnc_rdsyssoft2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSS2 a);

        /* read CNC module configuration information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmdlconfig")]
        public static extern short cnc_rdmdlconfig(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMDLC a);

        /* read CNC module configuration information 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmdlconfig2")]
        public static extern short cnc_rdmdlconfig2(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read processing condition file (processing data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpscdproc")]
        public static extern short cnc_rdpscdproc(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPSCD c);

        /* write processing condition file (processing data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpscdproc")]
        public static extern short cnc_wrpscdproc(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPSCD c);

        /* read processing condition file (piercing data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpscdpirc")]
        public static extern short cnc_rdpscdpirc(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPIRC c);

        /* write processing condition file (piercing data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpscdpirc")]
        public static extern short cnc_wrpscdpirc(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPIRC c);

        /* read processing condition file (edging data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpscdedge")]
        public static extern short cnc_rdpscdedge(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBEDGE c);

        /* write processing condition file (edging data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpscdedge")]
        public static extern short cnc_wrpscdedge(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBEDGE c);

        /* read processing condition file (slope data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpscdslop")]
        public static extern short cnc_rdpscdslop(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLOP c);

        /* write processing condition file (slope data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpscdslop")]
        public static extern short cnc_wrpscdslop(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLOP c);

        /* read power controll duty data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlpwrdty")]
        public static extern short cnc_rdlpwrdty(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBLPWDT a);

        /* write power controll duty data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlpwrdty")]
        public static extern short cnc_wrlpwrdty(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBLPWDT a);

        /* read laser power data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlpwrdat")]
        public static extern short cnc_rdlpwrdat(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLOPDT a);

        /* read power complement */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlpwrcpst")]
        public static extern short cnc_rdlpwrcpst(ushort FlibHndl, out short a);

        /* write power complement */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlpwrcpst")]
        public static extern short cnc_wrlpwrcpst(ushort FlibHndl, short a);

        /* read laser assist gas selection */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlagslt")]
        public static extern short cnc_rdlagslt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBLAGSL a);

        /* write laser assist gas selection */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlagslt")]
        public static extern short cnc_wrlagslt(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBLAGSL a);

        /* read laser assist gas flow */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlagst")]
        public static extern short cnc_rdlagst(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBLAGST a);

        /* write laser assist gas flow */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlagst")]
        public static extern short cnc_wrlagst(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBLAGST a);

        /* read laser power for edge processing */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdledgprc")]
        public static extern short cnc_rdledgprc(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBLEGPR a);

        /* write laser power for edge processing */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrledgprc")]
        public static extern short cnc_wrledgprc(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBLEGPR a);

        /* read laser power for piercing */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlprcprc")]
        public static extern short cnc_rdlprcprc(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBLPCPR a);

        /* write laser power for piercing */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlprcprc")]
        public static extern short cnc_wrlprcprc(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBLPCPR a);

        /* read laser command data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlcmddat")]
        public static extern short cnc_rdlcmddat(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLCMDT a);

        /* read displacement */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdldsplc")]
        public static extern short cnc_rdldsplc(ushort FlibHndl, out short a);

        /* write displacement */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrldsplc")]
        public static extern short cnc_wrldsplc(ushort FlibHndl, short a);

        /* read error for axis z */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlerrz")]
        public static extern short cnc_rdlerrz(ushort FlibHndl, out short a);

        /* read active number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlactnum")]
        public static extern short cnc_rdlactnum(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLACTN a);

        /* read laser comment */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlcmmt")]
        public static extern short cnc_rdlcmmt(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLCMMT a);

        /* read laser power select */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlpwrslt")]
        public static extern short cnc_rdlpwrslt(ushort FlibHndl, out short a);

        /* write laser power select */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlpwrslt")]
        public static extern short cnc_wrlpwrslt(ushort FlibHndl, short a);

        /* read laser power controll */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlpwrctrl")]
        public static extern short cnc_rdlpwrctrl(ushort FlibHndl, out short a);

        /* write laser power controll */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrlpwrctrl")]
        public static extern short cnc_wrlpwrctrl(ushort FlibHndl, short a);

        /* read power correction factor history data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpwofsthis")]
        public static extern short cnc_rdpwofsthis(ushort FlibHndl,
            int a, ref int b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPWOFST c);

        /* read management time */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmngtime")]
        public static extern short cnc_rdmngtime(ushort FlibHndl,
            int a, ref int b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBMNGTIME c);

        /* write management time */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmngtime")]
        public static extern short cnc_wrmngtime(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBMNGTIME b);

        /* read data related to electrical discharge at power correction ends */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddischarge")]
        public static extern short cnc_rddischarge(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDISCHRG a);

        /* read alarm history data related to electrical discharg */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddischrgalm")]
        public static extern short cnc_rddischrgalm(ushort FlibHndl,
            int a, ref int b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDISCHRGALM c);

        /* get date and time from cnc */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_gettimer")]
        public static extern short cnc_gettimer(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTIMER a);

        /* set date and time for cnc */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_settimer")]
        public static extern short cnc_settimer(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTIMER a);

        /* read timer data from cnc */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtimer")]
        public static extern short cnc_rdtimer(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTIME b);

        /* write timer data for cnc */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtimer")]
        public static extern short cnc_wrtimer(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTIME b);

        /* read tool controll data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtlctldata")]
        public static extern short cnc_rdtlctldata(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLCTL a);

        /* write tool controll data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtlctldata")]
        public static extern short cnc_wrtlctldata(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLCTL a);

        /* read tool data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtooldata")]
        public static extern short cnc_rdtooldata(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLDT c);

        /* read tool data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtooldata")]
        public static extern short cnc_wrtooldata(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLDT c);

        /* read multi tool data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmultitldt")]
        public static extern short cnc_rdmultitldt(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBMLTTL c);

        /* write multi tool data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmultitldt")]
        public static extern short cnc_wrmultitldt(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBMLTTL c);

        /* read multi tap data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmtapdata")]
        public static extern short cnc_rdmtapdata(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBMTAP c);

        /* write multi tap data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmtapdata")]
        public static extern short cnc_wrmtapdata(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBMTAP c);

        /* read multi-piece machining number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmultipieceno")]
        public static extern short cnc_rdmultipieceno(ushort FlibHndl, out int a);

        /* read tool information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtoolinfo")]
        public static extern short cnc_rdtoolinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPTLINF a);

        /* read safetyzone data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsafetyzone")]
        public static extern short cnc_rdsafetyzone(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSAFE c);

        /* write safetyzone data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrsafetyzone")]
        public static extern short cnc_wrsafetyzone(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSAFE c);

        /* read toolzone data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdtoolzone")]
        public static extern short cnc_rdtoolzone(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBTLZN c);

        /* write toolzone data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrtoolzone")]
        public static extern short cnc_wrtoolzone(ushort FlibHndl,
            short a, ref short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBTLZN c);

        /* read active toolzone data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdacttlzone")]
        public static extern short cnc_rdacttlzone(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBACTTLZN a);

        /* read setzone number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsetzone")]
        public static extern short cnc_rdsetzone(ushort FlibHndl, out short a);

        /* write setzone number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrsetzone")]
        public static extern short cnc_wrsetzone(ushort FlibHndl, short a);

        /* read block restart information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdbrstrinfo")]
        public static extern short cnc_rdbrstrinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBBRS a);

        /* read menu switch signal */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmenuswitch")]
        public static extern short cnc_rdmenuswitch(ushort FlibHndl, out short a);

        /* write menu switch signal */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrmenuswitch")]
        public static extern short cnc_wrmenuswitch(ushort FlibHndl, short a, short b);

        /* read tool radius offset for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdradofs")]
        public static extern short cnc_rdradofs(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBROFS a);

        /* read tool length offset for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdlenofs")]
        public static extern short cnc_rdlenofs(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBLOFS a);

        /* read fixed cycle for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdfixcycle")]
        public static extern short cnc_rdfixcycle(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBFIX a);

        /* read coordinate rotate for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcdrotate")]
        public static extern short cnc_rdcdrotate(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBROT a);

        /* read 3D coordinate convert for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dcdcnv")]
        public static extern short cnc_rd3dcdcnv(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB3DCD a);

        /* read programable mirror image for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdmirimage")]
        public static extern short cnc_rdmirimage(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBMIR a);

        /* read scaling for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdscaling")]
        public static extern short cnc_rdscaling(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSCL a);

        /* read 3D tool offset for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dtofs")]
        public static extern short cnc_rd3dtofs(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB3DTO a);

        /* read tool position offset for position data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdposofs")]
        public static extern short cnc_rdposofs(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPOFS a);

        /* read hpcc setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhpccset")]
        public static extern short cnc_rdhpccset(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBHPST a);

        /* write hpcc setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhpccset")]
        public static extern short cnc_wrhpccset(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBHPST a);

        /* hpcc data auto setting data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_hpccatset")]
        public static extern short cnc_hpccatset(ushort FlibHndl);

        /* read hpcc tuning data ( parameter input ) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhpcctupr")]
        public static extern short cnc_rdhpcctupr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBHPPR a);

        /* write hpcc tuning data ( parameter input ) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhpcctupr")]
        public static extern short cnc_wrhpcctupr(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBHPPR a);

        /* read hpcc tuning data ( acc input ) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdhpcctuac")]
        public static extern short cnc_rdhpcctuac(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBHPAC a);

        /* write hpcc tuning data ( acc input ) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrhpcctuac")]
        public static extern short cnc_wrhpcctuac(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBHPAC a);

        /* hpcc data auto tuning */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_hpccattune")]
        public static extern short cnc_hpccattune(ushort FlibHndl, short a, out short b);

        /* read hpcc fine level */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_hpccactfine")]
        public static extern short cnc_hpccactfine(ushort FlibHndl, out short a);

        /* select hpcc fine level */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_hpccselfine")]
        public static extern short cnc_hpccselfine(ushort FlibHndl, short a);

        /* read active fixture offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdactfixofs")]
        public static extern short cnc_rdactfixofs(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBZOFS b);

        /* read fixture offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdfixofs")]
        public static extern short cnc_rdfixofs(ushort FlibHndl,
            short a, short b, short c, short d, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBZOR e);

        /* write fixture offset */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrfixofs")]
        public static extern short cnc_wrfixofs(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBZOR b);

        /* read tip of tool for 3D handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dtooltip")]
        public static extern short cnc_rd3dtooltip(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB3DHDL a);

        /* read pulse for 3D handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dpulse")]
        public static extern short cnc_rd3dpulse(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB3DPLS a);

        /* read move overrlap of tool for 3D handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dmovrlap")]
        public static extern short cnc_rd3dmovrlap(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODB3DHDL a);

        /* read change offset for 3D handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rd3dofschg")]
        public static extern short cnc_rd3dofschg(ushort FlibHndl, ref int a);

        /* clear pulse and change offset for 3D handle */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clr3dplsmov")]
        public static extern short cnc_clr3dplsmov(ushort FlibHndl, short a);

        /* cycle start */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_start")]
        public static extern short cnc_start(ushort FlibHndl);

        /* reset CNC */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_reset")]
        public static extern short cnc_reset(ushort FlibHndl);

        /* reset CNC 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_reset2")]
        public static extern short cnc_reset2(ushort FlibHndl);

        /* read axis name */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdaxisname")]
        public static extern short cnc_rdaxisname(ushort FlibHndl,
            ref short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXISNAME b);

        /* read spindle name */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdspdlname")]
        public static extern short cnc_rdspdlname(ushort FlibHndl,
            ref short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSPDLNAME b);

        /* read extended axis name */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_exaxisname")]
        public static extern short cnc_exaxisname(ushort FlibHndl,
            short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBEXAXISNAME c);

        /* read SRAM variable area for C language executor */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcexesram")]
        public static extern short cnc_rdcexesram(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* write SRAM variable area for C language executor */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrcexesram")]
        public static extern short cnc_wrcexesram(ushort FlibHndl, int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, ref int c);

        /* read maximum size and linear address of SRAM variable area for C language executor */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cexesraminfo")]
        public static extern short cnc_cexesraminfo(ushort FlibHndl, out short a, out int b, out int c);

        /* read maximum size of SRAM variable area for C language executor */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cexesramsize")]
        public static extern short cnc_cexesramsize(ushort FlibHndl, out int a);

        /* read additional workpiece coordinate systems number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcoordnum")]
        public static extern short cnc_rdcoordnum(ushort FlibHndl, out short a);

        /* converts from FANUC code to Shift JIS code */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_ftosjis")]
        public static extern short cnc_ftosjis(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* Set the unsolicited message parameters */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrunsolicprm")]
        public static extern short cnc_wrunsolicprm(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBUNSOLIC b);

        /* Get the unsolicited message parameters */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdunsolicprm")]
        public static extern short cnc_rdunsolicprm(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBUNSOLIC b);

        /* Start of unsolicited message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_unsolicstart")]
        public static extern short cnc_unsolicstart(ushort FlibHndl, short a, int hWnd, uint c, short d, out short e);

        /* End of unsolicited message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_unsolicstop")]
        public static extern short cnc_unsolicstop(ushort FlibHndl, short a);

        /* Reads the unsolicited message data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdunsolicmsg")]
        public static extern short cnc_rdunsolicmsg(short a, [In, Out] IDBUNSOLICMSG b);

        /* read machine specific maintenance item */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpm_mcnitem")]
        public static extern short cnc_rdpm_mcnitem(ushort FlibHndl, short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBITEM c);

        /* write machine specific maintenance item */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpm_mcnitem")]
        public static extern short cnc_wrpm_mcnitem(ushort FlibHndl, short a, short b, [In, MarshalAs(UnmanagedType.LPStruct)] IODBITEM c);

        /* read cnc maintenance item */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpm_cncitem")]
        public static extern short cnc_rdpm_cncitem(ushort FlibHndl, short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBITEM c);

        /* read maintenance item status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdpm_item")]
        public static extern short cnc_rdpm_item(ushort FlibHndl, short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMAINTE c);

        /* write maintenance item status */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrpm_item")]
        public static extern short cnc_wrpm_item(ushort FlibHndl, short a, short b, short c, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPMAINTE d);

        /* Display of optional message */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_dispoptmsg")]
        public static extern short cnc_dispoptmsg(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* Reading of answer for optional message display */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_optmsgans")]
        public static extern short cnc_optmsgans(ushort FlibHndl, out short a);

        /* Get CNC Model */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getcncmodel")]
        public static extern short cnc_getcncmodel(ushort FlibHndl, out short a);

        /* read number of repeats */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrepeatval")]
        public static extern short cnc_rdrepeatval(ushort FlibHndl, out int a);

        /* read CNC system hard info */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsyshard")]
        public static extern short cnc_rdsyshard(ushort FlibHndl, short a, ref short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSH c);

        /* read CNC system soft series and version (3) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdsyssoft3")]
        public static extern short cnc_rdsyssoft3(ushort FlibHndl, short a, ref short b, out short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSS3 d);

        /* read digit of program number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_progdigit")]
        public static extern short cnc_progdigit(ushort FlibHndl, out short a);

        /* read CNC system path information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sysinfo_ex")]
        public static extern short cnc_sysinfo_ex(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSYSEX a);

        /*------------------*/
        /* CNC : SERCOS I/F */
        /*------------------*/

        /* Get reservation of service channel for SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrsvchnl")]
        public static extern short cnc_srcsrsvchnl(ushort FlibHndl);

        /* Read ID information of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrdidinfo")]
        public static extern short cnc_srcsrdidinfo(ushort FlibHndl,
            int a, short b, short c, [Out, MarshalAs(UnmanagedType.AsAny)] IODBIDINF d);

        /* Write ID information of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcswridinfo")]
        public static extern short cnc_srcswridinfo(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBIDINF a);

        /* Start of reading operation data from drive of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsstartrd")]
        public static extern short cnc_srcsstartrd(ushort FlibHndl, int a, short b);

        /* Start of writing operation data to drive of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsstartwrt")]
        public static extern short cnc_srcsstartwrt(ushort FlibHndl, int a, short b);

        /* Stop of reading/writing operation data from/to drive of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsstopexec")]
        public static extern short cnc_srcsstopexec(ushort FlibHndl);

        /* Get execution status of reading/writing operation data of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrdexstat")]
        public static extern short cnc_srcsrdexstat(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSRCSST a);

        /* Read operation data from data buffer for SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrdopdata")]
        public static extern short cnc_srcsrdopdata(ushort FlibHndl, int a, ref int b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* Write operation data to data buffer for SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcswropdata")]
        public static extern short cnc_srcswropdata(ushort FlibHndl, int a, int b, [In, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* Free reservation of service channel for SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsfreechnl")]
        public static extern short cnc_srcsfreechnl(ushort FlibHndl);

        /* Read drive assign of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrdlayout")]
        public static extern short cnc_srcsrdlayout(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSRCSLYT a);

        /* Read communication phase of drive of SERCOS I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_srcsrddrvcp")]
        public static extern short cnc_srcsrddrvcp(ushort FlibHndl, out short a);


        /*----------------------------*/
        /* CNC : Graphic command data */
        /*----------------------------*/

        /* Start drawing position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startdrawpos")]
        public static extern short cnc_startdrawpos(ushort FlibHndl);

        /* Stop drawing position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopdrawpos")]
        public static extern short cnc_stopdrawpos(ushort FlibHndl);

        /* Start dynamic graphic */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startdyngrph")]
        public static extern short cnc_startdyngrph(ushort FlibHndl);

        /* Stop dynamic graphic */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopdyngrph")]
        public static extern short cnc_stopdyngrph(ushort FlibHndl);

        /* Read graphic command data */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrphcmd")]
        public static extern short cnc_rdgrphcmd(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* Update graphic command read pointer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrgrphcmdptr")]
        public static extern short cnc_wrgrphcmdptr(ushort FlibHndl, short a);

        /* Read cancel flag */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdgrphcanflg")]
        public static extern short cnc_rdgrphcanflg(ushort FlibHndl, out short a);

        /* Clear graphic command */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clrgrphcmd")]
        public static extern short cnc_clrgrphcmd(ushort FlibHndl);


        /*---------------------------*/
        /* CNC : Servo learning data */
        /*---------------------------*/

        /* Servo learning data read start */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtstartrd")]
        public static extern short cnc_svdtstartrd(ushort FlibHndl, short a);

        /* Servo learning data write start */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtstartwr")]
        public static extern short cnc_svdtstartwr(ushort FlibHndl, short a);

        /* Servo learning data read end */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtendrd")]
        public static extern short cnc_svdtendrd(ushort FlibHndl);

        /* Servo learning data write end */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtendwr")]
        public static extern short cnc_svdtendwr(ushort FlibHndl);

        /* Servo learning data read/write stop */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtstopexec")]
        public static extern short cnc_svdtstopexec(ushort FlibHndl);

        /* Servo learning data read from I/F buffer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtrddata")]
        public static extern short cnc_svdtrddata(ushort FlibHndl,
            out short a, ref int b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* Servo learning data write to I/F buffer */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_svdtwrdata")]
        public static extern short cnc_svdtwrdata(ushort FlibHndl,
            out short a, ref int b, [In, MarshalAs(UnmanagedType.AsAny)] Object c);


        /*----------------------------*/
        /* CNC : Servo Guide          */
        /*----------------------------*/
        /* Servo Guide (Channel data set) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdsetchnl")]
        public static extern short cnc_sdsetchnl(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IDBCHAN b);

        /* Servo Guide (Channel data clear) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdclrchnl")]
        public static extern short cnc_sdclrchnl(ushort FlibHndl);

        /* Servo Guide (Sampling start) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdstartsmpl")]
        public static extern short cnc_sdstartsmpl(ushort FlibHndl, short a, int b, [Out, MarshalAs(UnmanagedType.AsAny)] Object c);

        /* Servo Guide (Sampling cancel) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdcancelsmpl")]
        public static extern short cnc_sdcancelsmpl(ushort FlibHndl);

        /* Servo Guide (read Sampling data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdreadsmpl")]
        public static extern short cnc_sdreadsmpl(ushort FlibHndl,
            out short a, int b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSD c);

        /* Servo Guide (Sampling end) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdendsmpl")]
        public static extern short cnc_sdendsmpl(ushort FlibHndl);

        /* Servo Guide (read 1 shot data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sdread1shot")]
        public static extern short cnc_sdread1shot(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* Servo feedback data (Channel data set) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbsetchnl")]
        public static extern short cnc_sfbsetchnl(ushort FlibHndl,
            short a, int b, [In, MarshalAs(UnmanagedType.LPStruct)] IDBSFBCHAN c);

        /* Servo feedback data (Channel data clear) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbclrchnl")]
        public static extern short cnc_sfbclrchnl(ushort FlibHndl);

        /* Servo feedback data (Sampling start) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbstartsmpl")]
        public static extern short cnc_sfbstartsmpl(ushort FlibHndl, short a, int b);

        /* Servo feedback data (Sampling cancel) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbcancelsmpl")]
        public static extern short cnc_sfbcancelsmpl(ushort FlibHndl);

        /* Servo feedback data (read Sampling data) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbreadsmpl")]
        public static extern short cnc_sfbreadsmpl(ushort FlibHndl,
            out short a, int b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSD c);

        /* Servo feedback data (Sampling end) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_sfbendsmpl")]
        public static extern short cnc_sfbendsmpl(ushort FlibHndl);


        /*----------------------------*/
        /* CNC : NC display function  */
        /*----------------------------*/

        /* Start NC display */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startnccmd")]
        public static extern short cnc_startnccmd(ushort FlibHndl);

        /* Start NC display (2) */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startnccmd2")]
        public static extern short cnc_startnccmd2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* Stop NC display */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopnccmd")]
        public static extern short cnc_stopnccmd(ushort FlibHndl);

        /* Get NC display mode */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getdspmode")]
        public static extern short cnc_getdspmode(ushort FlibHndl, out short a);


        /*------------------------------------*/
        /* CNC : Remote diagnostics function  */
        /*------------------------------------*/

        /* Start remote diagnostics function */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startrmtdgn")]
        public static extern short cnc_startrmtdgn(ushort FlibHndl);

        /* Stop remote diagnostics function */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stoprmtdgn")]
        public static extern short cnc_stoprmtdgn(ushort FlibHndl);

        /* Read data from remote diagnostics I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdrmtdgn")]
        public static extern short cnc_rdrmtdgn(ushort FlibHndl, out int a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* Write data to remote diagnostics I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrrmtdgn")]
        public static extern short cnc_wrrmtdgn(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* Set CommStatus of remote diagnostics I/F area */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrcommstatus")]
        public static extern short cnc_wrcommstatus(ushort FlibHndl, short a);

        /* Check remote diagnostics I/F */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_chkrmtdgn")]
        public static extern short cnc_chkrmtdgn(ushort FlibHndl);


        /*-------------------------*/
        /* CNC : FS18-LN function  */
        /*-------------------------*/

        /* read allowance */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allowance")]
        public static extern short cnc_allowance(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);

        /* read allowanced state */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allowcnd")]
        public static extern short cnc_allowcnd(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBCAXIS c);

        /* set work zero */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_workzero")]
        public static extern short cnc_workzero(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBZOFS b);

        /* set slide position */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_slide")]
        public static extern short cnc_slide(ushort FlibHndl,
            short a, short b, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBAXIS c);


        /*----------------------------------*/
        /* CNC: Teaching data I/F function  */
        /*----------------------------------*/

        /* Teaching data get start */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_startgetdgdat")]
        public static extern short cnc_startgetdgdat(ushort FlibHndl);

        /* Teaching data get stop */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_stopgetdgdat")]
        public static extern short cnc_stopgetdgdat(ushort FlibHndl);

        /* Teaching data read */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rddgdat")]
        public static extern short cnc_rddgdat(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* Teaching data read pointer write */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrdgdatptr")]
        public static extern short cnc_wrdgdatptr(ushort FlibHndl, short a);

        /* Teaching data clear */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_clrdgdat")]
        public static extern short cnc_clrdgdat(ushort FlibHndl);


        /*---------------------------------*/
        /* CNC : C-EXE SRAM file function  */
        /*---------------------------------*/

        /* open C-EXE SRAM file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_opencexefile")]
        public static extern short cnc_opencexefile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, short b, short c);

        /* close C-EXE SRAM file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_closecexefile")]
        public static extern short cnc_closecexefile(ushort FlibHndl);

        /* read C-EXE SRAM file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdcexefile")]
        public static extern short cnc_rdcexefile(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a, ref uint b);

        /* write C-EXE SRAM file */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_wrcexefile")]
        public static extern short cnc_wrcexefile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, ref uint b);

        /* read C-EXE SRAM disk directory */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_cexedirectory")]
        public static extern short cnc_cexedirectory(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.AsAny)] Object a, ref ushort b, ushort c, [Out, MarshalAs(UnmanagedType.LPStruct)] CFILEINFO d);


        /*-----*/
        /* PMC */
        /*-----*/

        /* read message from PMC to MMC */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdmsg")]
        public static extern short pmc_rdmsg(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write message from MMC to PMC */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrmsg")]
        public static extern short pmc_wrmsg(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read message from PMC to MMC(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_crdmsg")]
        public static extern short pmc_crdmsg(ushort FlibHndl, ref short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write message from MMC to PMC(conditional) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_cwrmsg")]
        public static extern short pmc_cwrmsg(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read PMC data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcrng")]
        public static extern short pmc_rdpmcrng(ushort FlibHndl,
            short a, short b, ushort c, ushort d, ushort e, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMC0 f);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcrng")]
        public static extern short pmc_rdpmcrng(ushort FlibHndl,
            short a, short b, ushort c, ushort d, ushort e, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMC1 f);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcrng")]
        public static extern short pmc_rdpmcrng(ushort FlibHndl,
            short a, short b, ushort c, ushort d, ushort e, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMC2 f);

        /* write PMC data(area specified) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcrng")]
        public static extern short pmc_wrpmcrng(ushort FlibHndl, ushort a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPMC0 b);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcrng")]
        public static extern short pmc_wrpmcrng(ushort FlibHndl, ushort a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPMC1 b);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcrng")]
        public static extern short pmc_wrpmcrng(ushort FlibHndl, ushort a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPMC2 b);

        /* read data from extended backup memory */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdkpm")]
        public static extern short pmc_rdkpm(ushort FlibHndl, uint a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, ushort c);

        /* write data to extended backup memory */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrkpm")]
        public static extern short pmc_wrkpm(ushort FlibHndl, uint a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, ushort c);

        /* read data from extended backup memory 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdkpm2")]
        public static extern short pmc_rdkpm2(ushort FlibHndl, uint a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b, uint c);

        /* write data to extended backup memory 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrkpm2")]
        public static extern short pmc_wrkpm2(ushort FlibHndl, uint a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, uint c);

        /* read maximum size of extended backup memory */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_kpmsiz")]
        public static extern short pmc_kpmsiz(ushort FlibHndl, out uint a);

        /* read informations of PMC data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcinfo")]
        public static extern short pmc_rdpmcinfo(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMCINF b);

        /* read PMC parameter data table contorol data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdcntldata")]
        public static extern short pmc_rdcntldata(ushort FlibHndl,
            short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMCCNTL d);

        /* write PMC parameter data table contorol data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrcntldata")]
        public static extern short pmc_wrcntldata(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPMCCNTL b);

        /* read PMC parameter data table contorol data group number */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdcntlgrp")]
        public static extern short pmc_rdcntlgrp(ushort FlibHndl, out short a);

        /* write PMC parameter data table contorol data group number */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrcntlgrp")]
        public static extern short pmc_wrcntlgrp(ushort FlibHndl, short a);

        /* read PMC alarm message */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdalmmsg")]
        public static extern short pmc_rdalmmsg(ushort FlibHndl,
            short a, ref short b, out short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMCALM d);

        /* get detail error for pmc */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_getdtailerr")]
        public static extern short pmc_getdtailerr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMCERR a);

        /* read PMC memory data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcmem")]
        public static extern short pmc_rdpmcmem(ushort FlibHndl,
            short a, int b, int c, [Out, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* write PMC memory data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcmem")]
        public static extern short pmc_wrpmcmem(ushort FlibHndl,
            short a, int b, int c, [In, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* read PMC-SE memory data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcsemem")]
        public static extern short pmc_rdpmcsemem(ushort FlibHndl,
            short a, int b, int c, [Out, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* write PMC-SE memory data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcsemem")]
        public static extern short pmc_wrpmcsemem(ushort FlibHndl,
            short a, int b, int c, [In, MarshalAs(UnmanagedType.AsAny)] Object d);

        /* read pmc title data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmctitle")]
        public static extern short pmc_rdpmctitle(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMCTITLE a);

        /* read PMC parameter start */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdprmstart")]
        public static extern short pmc_rdprmstart(ushort FlibHndl);

        /* read PMC parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcparam")]
        public static extern short pmc_rdpmcparam(ushort FlibHndl, ref int a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read PMC parameter end */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdprmend")]
        public static extern short pmc_rdprmend(ushort FlibHndl);

        /* write PMC parameter start */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrprmstart")]
        public static extern short pmc_wrprmstart(ushort FlibHndl);

        /* write PMC parameter */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrpmcparam")]
        public static extern short pmc_wrpmcparam(ushort FlibHndl, ref int a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write PMC parameter end */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wrprmend")]
        public static extern short pmc_wrprmend(ushort FlibHndl);

        /* read PMC data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcrng_ext")]
        public static extern short pmc_rdpmcrng_ext(ushort FlibHndl,
            short a, [In, Out, MarshalAs(UnmanagedType.LPStruct)] IODBPMCEXT b);

        /* write PMC I/O link assigned data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_wriolinkdat")]
        public static extern short pmc_wriolinkdat(ushort FlibHndl, uint a, [In, MarshalAs(UnmanagedType.AsAny)] Object b, uint c);

        /* read PMC address information */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_rdpmcaddr")]
        public static extern short pmc_rdpmcaddr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPMCADR a);

        /* select PMC unit */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_select_pmc_unit")]
        public static extern short pmc_select_pmc_unit(ushort FlibHndl, int a);

        /* get current PMC unit */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_get_current_pmc_unit")]
        public static extern short pmc_get_current_pmc_unit(ushort FlibHndl, ref int a);

        /* get number of PMC */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_get_number_of_pmc")]
        public static extern short pmc_get_number_of_pmc(ushort FlibHndl, ref int a);

        /* get PMC unit types */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_get_pmc_unit_types")]
        public static extern short pmc_get_pmc_unit_types(ushort FlibHndl, int[] a, ref int b);

        /* set PMC Timer type */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_set_timer_type")]
        public static extern short pmc_set_timer_type(ushort FlibHndl, ushort a, ushort b, ref short c);

        /* get PMC Timer type */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_get_timer_type")]
        public static extern short pmc_get_timer_type(ushort FlibHndl, ushort a, ushort b, ref short c);

        /*----------------------------*/
        /* PMC : PROFIBUS function    */
        /*----------------------------*/

        /* read PROFIBUS configration data */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdconfig")]
        public static extern short pmc_prfrdconfig(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBPRFCNF a);

        /* read bus parameter for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdbusprm")]
        public static extern short pmc_prfrdbusprm(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBBUSPRM a);

        /* write bus parameter for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrbusprm")]
        public static extern short pmc_prfwrbusprm(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBBUSPRM a);

        /* read slave parameter for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvprm")]
        public static extern short pmc_prfrdslvprm(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM b);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvprm")]
        public static extern short pmc_prfrdslvprm(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM2 b);

        /* write slave parameter for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrslvprm")]
        public static extern short pmc_prfwrslvprm(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM b);
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrslvprm")]
        public static extern short pmc_prfwrslvprm(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM2 b);

        /* read allocation address for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdallcadr")]
        public static extern short pmc_prfrdallcadr(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBPRFADR b);

        /* set allocation address for master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrallcadr")]
        public static extern short pmc_prfwrallcadr(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBPRFADR b);

        /* read allocation address for slave function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvaddr")]
        public static extern short pmc_prfrdslvaddr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLVADR a);

        /* set allocation address for slave function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrslvaddr")]
        public static extern short pmc_prfwrslvaddr(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLVADR a);

        /* read status for slave function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvstat")]
        public static extern short pmc_prfrdslvstat(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBSLVST a);

        /* Reads slave index data of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvid")]
        public static extern short pmc_prfrdslvid(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLVID b);

        /* Writes slave index data of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrslvid")]
        public static extern short pmc_prfwrslvid(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLVID b);

        /* Reads slave parameter of master function(2) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdslvprm2")]
        public static extern short pmc_prfrdslvprm2(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM3 b);

        /* Writes slave parameter of master function(2) */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrslvprm2")]
        public static extern short pmc_prfwrslvprm2(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBSLVPRM3 b);

        /* Reads DI/DO parameter of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrddido")]
        public static extern short pmc_prfrddido(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBDIDO b);

        /* Writes DI/DO parameter of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrdido")]
        public static extern short pmc_prfwrdido(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.LPStruct)] IODBDIDO b);

        /* Reads indication address of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdindiadr")]
        public static extern short pmc_prfrdindiadr(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBINDEADR a);

        /* Writes indication address of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwrindiadr")]
        public static extern short pmc_prfwrindiadr(ushort FlibHndl, [In, MarshalAs(UnmanagedType.LPStruct)] IODBINDEADR a);

        /* Reads operation mode of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfrdopmode")]
        public static extern short pmc_prfrdopmode(ushort FlibHndl, ref short a);

        /* Writes operation mode of master function */
        [DllImport("FWLIB32.dll", EntryPoint = "pmc_prfwropmode")]
        public static extern short pmc_prfwropmode(ushort FlibHndl, short a, ref short b);

        /*-----------------------------------------------*/
        /* DS : Data server & Ethernet board function    */
        /*-----------------------------------------------*/

        /* read the parameter of the Ethernet board */
        [DllImport("FWLIB32.dll", EntryPoint = "etb_rdparam")]
        public static extern short etb_rdparam(ushort FlibHndl,
            short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBETP_TCP b);
        [DllImport("FWLIB32.dll", EntryPoint = "etb_rdparam")]
        public static extern short etb_rdparam(ushort FlibHndl,
            short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBETP_HOST b);
        [DllImport("FWLIB32.dll", EntryPoint = "etb_rdparam")]
        public static extern short etb_rdparam(ushort FlibHndl,
            short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBETP_FTP b);
        [DllImport("FWLIB32.dll", EntryPoint = "etb_rdparam")]
        public static extern short etb_rdparam(ushort FlibHndl,
            short a, [Out, MarshalAs(UnmanagedType.LPStruct)] IODBETP_ETB b);

        /* write the parameter of the Ethernet board */
        [DllImport("FWLIB32.dll", EntryPoint = "etb_wrparam")]
        public static extern short etb_wrparam(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.LPStruct)] IODBETP_TCP a);
        [DllImport("FWLIB32.dll", EntryPoint = "etb_wrparam")]
        public static extern short etb_wrparam(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.LPStruct)] IODBETP_HOST a);
        [DllImport("FWLIB32.dll", EntryPoint = "etb_wrparam")]
        public static extern short etb_wrparam(ushort FlibHndl,
            [In, MarshalAs(UnmanagedType.LPStruct)] IODBETP_FTP a);

        /* read the error message of the Ethernet board */
        [DllImport("FWLIB32.dll", EntryPoint = "etb_rderrmsg")]
        public static extern short etb_rderrmsg(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBETMSG b);

        /* read the mode of the Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdmode")]
        public static extern short ds_rdmode(ushort FlibHndl, ref short a);

        /* write the mode of the Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrmode")]
        public static extern short ds_wrmode(ushort FlibHndl, short a);

        /* read information of the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhddinfo")]
        public static extern short ds_rdhddinfo(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHDDINF a);

        /* read the file list of the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhdddir")]
        public static extern short ds_rdhdddir(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, int b, out short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHDDDIR d);

        /* delete the file of the Data Serve's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_delhddfile")]
        public static extern short ds_delhddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* copy the file of the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_copyhddfile")]
        public static extern short ds_copyhddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* change the file name of the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_renhddfile")]
        public static extern short ds_renhddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* execute the PUT command of the FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_puthddfile")]
        public static extern short ds_puthddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* execute the MPUT command of the FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_mputhddfile")]
        public static extern short ds_mputhddfile(ushort hLib, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read information of the host */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhostinfo")]
        public static extern short ds_rdhostinfo(ushort FlibHndl, out int a, int b);

        /* read the file list of the host */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhostdir")]
        public static extern short ds_rdhostdir(ushort FlibHndl, short a, int b, out short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHOSTDIR d, int e);

        /* read the file list of the host 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhostdir2")]
        public static extern short ds_rdhostdir2(ushort FlibHndl, short a, int b, out short c, out int d, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBHOSTDIR e, int f);

        /* delete the file of the host */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_delhostfile")]
        public static extern short ds_delhostfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, int b);

        /* execute the GET command of the FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_gethostfile")]
        public static extern short ds_gethostfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* execute the MGET command of the FTP */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_mgethostfile")]
        public static extern short ds_mgethostfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the execution result */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdresult")]
        public static extern short ds_rdresult(ushort FlibHndl);

        /* stop the execution of the command */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_cancel")]
        public static extern short ds_cancel(ushort FlibHndl);

        /* read the file from the Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdncfile")]
        public static extern short ds_rdncfile(ushort FlibHndl, short a, [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* read the file from the Data Server 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdncfile2")]
        public static extern short ds_rdncfile2(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* write the file to the Data Server */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrncfile")]
        public static extern short ds_wrncfile(ushort FlibHndl, short a, int b);

        /* read the file name for the DNC operation in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rddnchddfile")]
        public static extern short ds_rddnchddfile(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* write the file name for the DNC operation in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrdnchddfile")]
        public static extern short ds_wrdnchddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the file name for the DNC operation in the host */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rddnchostfile")]
        public static extern short ds_rddnchostfile(ushort FlibHndl, out short a, [Out, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write the file name for the DNC operation in the host */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrdnchostfile")]
        public static extern short ds_wrdnchostfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the connecting host number */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdhostno")]
        public static extern short ds_rdhostno(ushort FlibHndl, out short a);

        /* read maintenance information */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdmntinfo")]
        public static extern short ds_rdmntinfo(ushort FlibHndl, short a, [Out, MarshalAs(UnmanagedType.LPStruct)] DSMNTINFO b);

        /* check the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_checkhdd")]
        public static extern short ds_checkhdd(ushort FlibHndl);

        /* format the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_formathdd")]
        public static extern short ds_formathdd(ushort FlibHndl);

        /* create the directory in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_makehdddir")]
        public static extern short ds_makehdddir(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* delete directory in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_delhdddir")]
        public static extern short ds_delhdddir(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* change the current directory */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_chghdddir")]
        public static extern short ds_chghdddir(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* execute the PUT command according to the list file */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_lputhddfile")]
        public static extern short ds_lputhddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* delete files according to the list file */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_ldelhddfile")]
        public static extern short ds_ldelhddfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* execute the GET command according to the list file */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_lgethostfile")]
        public static extern short ds_lgethostfile(ushort FlibHndl, [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the directory for M198 operation */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdm198hdddir")]
        public static extern short ds_rdm198hdddir(ushort FlibHndl, [Out, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* write the directory for M198 operation */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrm198hdddir")]
        public static extern short ds_wrm198hdddir(ushort FlibHndl);

        /* read the connecting host number for the M198 operation */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdm198host")]
        public static extern short ds_rdm198host(ushort FlibHndl, out short a);

        /* write the connecting host number for the M198 operation */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrm198host")]
        public static extern short ds_wrm198host(ushort FlibHndl);

        /* write the connecting host number */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrhostno")]
        public static extern short ds_wrhostno(ushort FlibHndl, short a);

        /* search string in data server program */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_searchword")]
        public static extern short ds_searchword(ushort FlibHndl,
                                  [In, MarshalAs(UnmanagedType.AsAny)] Object a);

        /* read the searching result */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_searchresult")]
        public static extern short ds_searchresult(ushort FlibHndl);

        /* read file in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_rdfile")]
        public static extern short ds_rdfile(ushort FlibHndl,
                                         [In, MarshalAs(UnmanagedType.AsAny)] Object a,
                                         [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /* write file in the Data Server's HDD */
        [DllImport("FWLIB32.dll", EntryPoint = "ds_wrfile")]
        public static extern short ds_wrfile(ushort FlibHndl,
                                         [In, MarshalAs(UnmanagedType.AsAny)] Object a,
                                         [In, MarshalAs(UnmanagedType.AsAny)] Object b);

        /*--------------------------*/
        /* HSSB multiple connection */
        /*--------------------------*/

        /* read number of node */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdnodenum")]
        public static extern short cnc_rdnodenum(out int a);

        /* read node informations */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdnodeinfo")]
        public static extern short cnc_rdnodeinfo(int a, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBNODE b);

        /* set default node number */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_setdefnode")]
        public static extern short cnc_setdefnode(int a);

        /* allocate library handle 2 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allclibhndl2")]
        public static extern short cnc_allclibhndl2(int node, out ushort FlibHndl);


        /*---------------------*/
        /* Ethernet connection */
        /*---------------------*/

        /* allocate library handle 3 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allclibhndl3")]
        public static extern short cnc_allclibhndl3([In, MarshalAs(UnmanagedType.AsAny)] Object ip,
            ushort port, int timeout, out ushort FlibHndl);

        /* allocate library handle 4 */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_allclibhndl4")]
        public static extern short cnc_allclibhndl4([In, MarshalAs(UnmanagedType.AsAny)] Object ip,
            ushort port, int timeout, uint id, out ushort FlibHndl);

        /* set timeout for socket */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_settimeout")]
        public static extern short cnc_settimeout(ushort FlibHndl, int a);

        /* reset all socket connection */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_resetconnect")]
        public static extern short cnc_resetconnect(ushort FlibHndl);

        /* get option state for FOCAS1/Ethernet */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_getfocas1opt")]
        public static extern short cnc_getfocas1opt(ushort FlibHndl, short a, out int b);

        /* read Ethernet board information */
        [DllImport("FWLIB32.dll", EntryPoint = "cnc_rdetherinfo")]
        public static extern short cnc_rdetherinfo(ushort FlibHndl, out short a, out short b);


    } // End for Focas1 class
}
