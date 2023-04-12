namespace PLC.ModBusMaster;

public enum MasterType
{
    Tcp = 0,
    Udp = 1,
    Rtu = 2,
    RtuOnTcp = 3,
    RtuOnUdp = 4,
    Ascii = 5,
    AsciiOnTcp = 6,
    AsciiOnUdp = 7,
}