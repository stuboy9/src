//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: ProtoHead.proto
namespace protocol
{
    [global::ProtoBuf.ProtoContract(Name=@"ENetworkMessage")]
    public enum ENetworkMessage
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"KEEP_ALIVE_SYNC", Value=0)]
      KEEP_ALIVE_SYNC = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"REGISTER_REQ", Value=1)]
      REGISTER_REQ = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"REGISTER_RSP", Value=2)]
      REGISTER_RSP = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGIN_REQ", Value=3)]
      LOGIN_REQ = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGIN_RSP", Value=4)]
      LOGIN_RSP = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PERSONALSETTINGS_REQ", Value=5)]
      PERSONALSETTINGS_REQ = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PERSONALSETTINGS_RSP", Value=6)]
      PERSONALSETTINGS_RSP = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_USERINFO_REQ", Value=7)]
      GET_USERINFO_REQ = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_USERINFO_RSP", Value=8)]
      GET_USERINFO_RSP = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ADD_FRIEND_REQ", Value=9)]
      ADD_FRIEND_REQ = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ADD_FRIEND_RSP", Value=10)]
      ADD_FRIEND_RSP = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DELETE_FRIEND_REQ", Value=11)]
      DELETE_FRIEND_REQ = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DELETE_FRIEND_RSP", Value=12)]
      DELETE_FRIEND_RSP = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"OFFLINE_SYNC", Value=13)]
      OFFLINE_SYNC = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_REQ", Value=14)]
      LOGOUT_REQ = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_RSP", Value=15)]
      LOGOUT_RSP = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_PERSONALINFO_REQ", Value=16)]
      GET_PERSONALINFO_REQ = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_PERSONALINFO_RSP", Value=17)]
      GET_PERSONALINFO_RSP = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_FRIEND_SYNC", Value=18)]
      CHANGE_FRIEND_SYNC = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SEND_CHAT_REQ", Value=19)]
      SEND_CHAT_REQ = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SEND_CHAT_RSP", Value=20)]
      SEND_CHAT_RSP = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RECEIVE_CHAT_SYNC", Value=21)]
      RECEIVE_CHAT_SYNC = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CREATE_GROUP_CHAT_REQ", Value=22)]
      CREATE_GROUP_CHAT_REQ = 22,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CREATE_GROUP_CHAT_RSP", Value=23)]
      CREATE_GROUP_CHAT_RSP = 23,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_GROUP_REQ", Value=24)]
      CHANGE_GROUP_REQ = 24,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_GROUP_RSP", Value=25)]
      CHANGE_GROUP_RSP = 25,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_GROUP_SYNC", Value=26)]
      CHANGE_GROUP_SYNC = 26,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_GROUP_INFO_REQ", Value=27)]
      GET_GROUP_INFO_REQ = 27,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_GROUP_INFO_RSP", Value=28)]
      GET_GROUP_INFO_RSP = 28,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_FILE_INFO_REQ", Value=29)]
      GET_FILE_INFO_REQ = 29,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_FILE_INFO_RSP", Value=30)]
      GET_FILE_INFO_RSP = 30,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ADD_FILE_REQ", Value=31)]
      ADD_FILE_REQ = 31,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ADD_FILE_RSP", Value=32)]
      ADD_FILE_RSP = 32,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DELETE_FILE_REQ", Value=33)]
      DELETE_FILE_REQ = 33,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DELETE_FILE_RSP", Value=34)]
      DELETE_FILE_RSP = 34,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_FILE_REQ", Value=35)]
      CHANGE_FILE_REQ = 35,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CHANGE_FILE_RSP", Value=36)]
      CHANGE_FILE_RSP = 36,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_MIRACAST_INFO_REQ", Value=37)]
      GET_MIRACAST_INFO_REQ = 37,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_MIRACAST_INFO_RSP", Value=38)]
      GET_MIRACAST_INFO_RSP = 38,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ACCREDIT_REQ", Value=39)]
      ACCREDIT_REQ = 39,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ACCREDIT_RSP", Value=40)]
      ACCREDIT_RSP = 40,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_EXE_INFO_REQ", Value=41)]
      GET_EXE_INFO_REQ = 41,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_EXE_INFO_RSP", Value=42)]
      GET_EXE_INFO_RSP = 42,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_DOWNLOAD_FILE_INFO_REQ", Value=43)]
      GET_DOWNLOAD_FILE_INFO_REQ = 43,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GET_DOWNLOAD_FILE_INFO_RSP", Value=44)]
      GET_DOWNLOAD_FILE_INFO_RSP = 44,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SEND_CODE", Value=45)]
      SEND_CODE = 45,
            
      [global::ProtoBuf.ProtoEnum(Name=@"HOLE_PUNCH_REQ", Value=46)]
      HOLE_PUNCH_REQ = 46,
            
      [global::ProtoBuf.ProtoEnum(Name=@"HOLE_PUNCH_RSP", Value=47)]
      HOLE_PUNCH_RSP = 47,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RELAY_INFO_REQ", Value=48)]
      RELAY_INFO_REQ = 48,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RELAY_INFO_RSP", Value=49)]
      RELAY_INFO_RSP = 49,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PC_KEEP_ALIVE", Value=50)]
      PC_KEEP_ALIVE = 50,
            
      [global::ProtoBuf.ProtoEnum(Name=@"P2P_CONTROL_REQ", Value=51)]
      P2P_CONTROL_REQ = 51,
            
      [global::ProtoBuf.ProtoEnum(Name=@"P2P_CONTROL_RSP", Value=52)]
      P2P_CONTROL_RSP = 52,
            
      [global::ProtoBuf.ProtoEnum(Name=@"P2P_KEEP_ALIVE", Value=53)]
      P2P_KEEP_ALIVE = 53,
            
      [global::ProtoBuf.ProtoEnum(Name=@"P2P_INIT_REQ", Value=54)]
      P2P_INIT_REQ = 54,
            
      [global::ProtoBuf.ProtoEnum(Name=@"P2P_INIT_RSP", Value=55)]
      P2P_INIT_RSP = 55
    }
  
}