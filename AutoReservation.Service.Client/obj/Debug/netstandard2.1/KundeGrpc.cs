// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: kunde.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace AutoReservation.Service.Grpc {
  public static partial class KundeService
  {
    static readonly string __ServiceName = "AutoReservation.KundeService";


    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::AutoReservation.Service.Grpc.KundeReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for KundeService</summary>
    public partial class KundeServiceClient : grpc::ClientBase<KundeServiceClient>
    {
      /// <summary>Creates a new client for KundeService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public KundeServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for KundeService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public KundeServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected KundeServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected KundeServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override KundeServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new KundeServiceClient(configuration);
      }
    }

  }
}
#endregion
