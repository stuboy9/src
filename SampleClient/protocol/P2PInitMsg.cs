//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: P2PInitMsg.proto
namespace protocol
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"P2PInitReq")]
  public partial class P2PInitReq : global::ProtoBuf.IExtensible
  {
    public P2PInitReq() {}
    
    private protocol.P2PInitReq.MsgFrom _msgFrom;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"msgFrom", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public protocol.P2PInitReq.MsgFrom msgFrom
    {
      get { return _msgFrom; }
      set { _msgFrom = value; }
    }
    private string _userName;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"MsgFrom")]
    public enum MsgFrom
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SERVER", Value=0)]
      SERVER = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PC", Value=1)]
      PC = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"APP", Value=2)]
      APP = 2
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"P2PInitRsp")]
  public partial class P2PInitRsp : global::ProtoBuf.IExtensible
  {
    public P2PInitRsp() {}
    
    private protocol.P2PInitRsp.MsgFrom _msgFrom;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"msgFrom", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public protocol.P2PInitRsp.MsgFrom msgFrom
    {
      get { return _msgFrom; }
      set { _msgFrom = value; }
    }
    private string _userName;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    private protocol.P2PInitRsp.Status _status;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public protocol.P2PInitRsp.Status status
    {
      get { return _status; }
      set { _status = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"MsgFrom")]
    public enum MsgFrom
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SERVER", Value=0)]
      SERVER = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PC", Value=1)]
      PC = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"APP", Value=2)]
      APP = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"Status")]
    public enum Status
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SUCCESS", Value=0)]
      SUCCESS = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"FAIL", Value=1)]
      FAIL = 1
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}