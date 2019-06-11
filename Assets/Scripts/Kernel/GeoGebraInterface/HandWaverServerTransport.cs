using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


namespace IMRE.HandWaver.Kernel
{
    
//TODO: convert to unitywebrequest
    public static class HandWaverServerTransport
    {
        
            String SID = "c55d57b8-8624-11e9-bc42-526af7764f64";
            static String version = "12321";

            public void getPNG(string path){

                RestClient client = new RestClient("http://localhost:8080/getPNG?sessionId="+SID);
                RestRequest request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                UnityEngine.Windows.File.WriteAllBytes(path ,response.RawBytes);
            }

            public void saveCurrSession(){
                RestClient client = new RestClient("http://localhost:8080/saveCurrSession");
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\n\t\"sessionId\": \"SID\"\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }

            public void getCurrSession(String path){
                RestClient client = new RestClient("http://localhost:8080/getCurrSession?sessionId="+SID);
                RestRequest request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                File.WriteAllBytes(path+"/"+SID+".ggb", response.RawBytes)
            }

            public void execCommand(String cmdString){
                RestClient client = new RestClient("http://localhost:8080/command");
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\n\t\"sessionId\": \"SID\",\n\t\"command\": "+cmdString+"\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }

            public void serverHandhake(){
                RestClient client = new RestClient("http://localhost:8080/handshake?sessionId="+SID+"&version="+version);
                RestRequest request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
            }
        
    }

}