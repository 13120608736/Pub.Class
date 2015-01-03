//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Pub.Class;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
#endif
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// QQ ��Ȩ��¼
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class QQOAuth : IOAuth {
        /// <summary>
        /// request_token
        /// </summary>
        public static readonly string request_token = "https://open.t.qq.com/cgi-bin/request_token";
        /// <summary>
        /// authorize
        /// </summary>
        public static readonly string authorize = "https://open.t.qq.com/cgi-bin/authorize";
        /// <summary>
        /// access_token
        /// </summary>
        public static readonly string access_token = "https://open.t.qq.com/cgi-bin/access_token";
        /// <summary>
        /// user_info
        /// </summary>
        public static readonly string user_info = "http://open.t.qq.com/api/user/info";
        /// <summary>
        /// friends_list
        /// </summary>
        public static readonly string friends_list = "http://open.t.qq.com/api/friends/idollist";
        /// <summary>
        /// add
        /// </summary>
        public static readonly string add = "http://open.t.qq.com/api/t/add";
        /// <summary>
        /// qq app ������Ϣ
        /// </summary>
        public static readonly ConfigInfo config = OAuthConfig.GetConfigInfo(OAuthEnum.qq);
        /// <summary>
        /// ȡ��Ȩ��¼URL
        /// </summary>
        /// <returns>��¼URL</returns>
        public string GetAuthUrl() {
            List<UrlParameter> param = new List<UrlParameter>();
            param.Add(new UrlParameter("oauth_callback", config.RedirectUrl.UrlEncode2()));
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign = new StringBuilder().Append("GET&")
                .Append(request_token.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, "", sbSign.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());

            string data = HttpHelper.SendGet(new StringBuilder().Append(request_token).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());

            string token = data.GetMatchingValues("oauth_token=(.+?)&", "oauth_token=", "&").FirstOrDefault() ?? "";
            string tokenSecret = data.GetMatchingValues("oauth_token_secret=(.+?)&", "oauth_token_secret=", "&").FirstOrDefault() ?? "";
            Session2.Set("oauth_token", token);
            Session2.Set("oauth_token_secret", tokenSecret);
            return authorize + "?oauth_token=" + token;
        }
        /// <summary>
        /// ȡ��¼�˺���Ϣ
        /// </summary>
        /// <returns>ȡ��¼�˺���Ϣ</returns>
        public UserInfo GetUserInfo() {
            UserInfo user = new UserInfo();
            string openid = Request2.GetQ("openid");
            string openkey = Request2.GetQ("openkey");

            List<UrlParameter> param = new List<UrlParameter>();
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_token", Request2.GetQ("oauth_token")));
            param.Add(new UrlParameter("oauth_verifier", Request2.GetQ("oauth_verifier")));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign = new StringBuilder().Append("GET&")
                .Append(access_token.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, Session2.Get("oauth_token_secret"), sbSign.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());
            string data = HttpHelper.SendGet(new StringBuilder().Append(access_token).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());

            user.Token = data.GetMatchingValues("oauth_token=(.+?)&", "oauth_token=", "&").FirstOrDefault() ?? "";
            user.Secret = data.GetMatchingValues("oauth_token_secret=(.+?)&", "oauth_token_secret=", "&").FirstOrDefault() ?? "";
            user.UserID = data.Substring(data.IndexOf("&name=") + 6);

            param.Clear();
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_token", user.Token));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Add(new UrlParameter("format", "json"));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign2 = new StringBuilder().Append("GET&")
                .Append(user_info.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, user.Secret, sbSign2.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());
            data = HttpHelper.SendGet(new StringBuilder().Append(user_info).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());
            
            user.Email = data.GetMatchingValues("\"email\":\"(.+?)\"", "\"email\":\"", "\"").FirstOrDefault() ?? "";
            user.Name = data.GetMatchingValues("\"nick\":\"(.+?)\"", "\"nick\":\"", "\"").FirstOrDefault() ?? "";
            user.Sex = (data.GetMatchingValues("\"sex\":(.+?),", "\"sex\":", ",").FirstOrDefault() ?? "") == "1" ? 1 : 0;
            user.Address = data.GetMatchingValues("\"location\":\"(.+?)\"", "\"location\":\"", "\"").FirstOrDefault() ?? "";

            //{"data":{"birth_day":31,"birth_month":3,"birth_year":1984,"city_code":"9","country_code":"1","edu":null,"email":"30133499@qq.com",
            //"fansnum":59,"head":"","idolnum":25,"introduction":"","isent":0,"isvip":0,"location":"δ֪","name":"cexo255","nick":"��","openid":"",
            //"province_code":"31","sex":1,"tag":null,"tweetnum":40,"verifyinfo":""},"errcode":0,"msg":"ok","ret":0}
            //Msg.WriteEnd(GetFriendsInfo(user.Token, user.Secret).ToJson());
            return user;
        }
        /// <summary>
        /// ȡ��¼�˺ź�����Ϣ
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <returns>ȡ��¼�˺ź�����Ϣ</returns>
        public IList<UserInfo> GetFriendsInfo(string accessToken, string accessSecret) {
            IList<UserInfo> list = new List<UserInfo>();
            bool isTrue = true; int count = 10; int page = 1;

            while (isTrue) {
                List<UrlParameter> param = new List<UrlParameter>();
                param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
                param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
                param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
                param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
                param.Add(new UrlParameter("oauth_token", accessToken));
                param.Add(new UrlParameter("oauth_version", "1.0"));
                param.Add(new UrlParameter("format", "json"));
                param.Add(new UrlParameter("reqnum", count));
                param.Add(new UrlParameter("startindex", count * (page-1)));
                param.Sort(new UrlParameterCompre());

                StringBuilder sbSign = new StringBuilder().Append("GET&")
                    .Append(friends_list.UrlEncode2())
                    .Append("&")
                    .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

                param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, accessSecret, sbSign.ToString()).UrlEncode2()));
                param.Sort(new UrlParameterCompre());
                string data = HttpHelper.SendGet(new StringBuilder().Append(friends_list).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());
                IList<string> userlist = data.GetMatchingValues("{\"city_code\"(.+?)}]}", "{\"city_code\"", "}]}");

                foreach (string info in userlist) {
                    UserInfo user = new UserInfo();
                    user.UserID = info.GetMatchingValues("\"name\":\"(.+?)\"", "\"name\":\"", "\"").FirstOrDefault() ?? "";
                    user.Email = info.GetMatchingValues("\"email\":\"(.+?)\"", "\"email\":\"", "\"").FirstOrDefault() ?? "";
                    user.Name = info.GetMatchingValues("\"nick\":\"(.+?)\"", "\"nick\":\"", "\"").FirstOrDefault() ?? "";
                    user.Sex = (info.GetMatchingValues("\"sex\":(.+?),", "\"sex\":", ",").FirstOrDefault() ?? "") == "1" ? 1 : 0;
                    user.Address = info.GetMatchingValues("\"location\":\"(.+?)\"", "\"location\":\"", "\"").FirstOrDefault() ?? "";
                    user.Header = info.GetMatchingValues("\"head\":\"(.+?)\"", "\"head\":\"", "\"").FirstOrDefault() ?? "";
                    list.Add(user);
                }

                if (userlist.IsNull() || userlist.Count == 0) isTrue = false;
                page++;
            };

            //{"data":{"hasnext":0,"info":[
            //{"city_code":"1","country_code":"1","fansnum":1517049,"head":"http://app.qlogo.cn/mbloghead/a234df2a8474d1e853be","idolnum":25,"isidol":true,"isvip":1,"location":"δ֪","name":"tncmayun","nick":"TNC����","openid":"","province_code":"33","tag":null,"tweet":[{"from":"��Ѷ΢��","id":"18650095129418","text":"���ż��˵����ᣬ����ͬ����ƣ��ί��������������ˣ������ˣ�������������������������Լ�������Ϊ��ʲô��ƾɶȥ�е���˵����Σ�Ҳ������׬��Ǯ�͸ù���������������һ���������û���������ɶ��ϵ�������������������˸����ɴ���裬����������һ�У��ݻ�һ�У��˺����޹����ף��Ա��ˣ���","timestamp":1318479818}]},
            //{"city_code":"","country_code":"1","fansnum":100,"head":"http://app.qlogo.cn/mbloghead/51d6d26e67012a6e069c","idolnum":136,"isidol":true,"isvip":0,"location":"δ֪","name":"pandorcai","nick":"����","openid":"","province_code":"31","tag":[{"id":"77846971042806542","name":"����"},{"id":"595526330566555944","name":"����̳"},{"id":"3116172981967911833","name":"��˯��"},{"id":"3428083006598920604","name":"����"},{"id":"4762518206506141882","name":"��Ϸ"},{"id":"7632343735443733612","name":"������"},{"id":"8143061372111998179","name":"����"},{"id":"9393533470027694381","name":"����Ӱ"},{"id":"12117796803083473608","name":"��ʳ"},{"id":"16511128160049158514","name":"����Ա"}],"tweet":[{"from":"QQǩ��","id":"71615048757565","text":"���������ŷ��������˲���ѽ","timestamp":1322884522}]},
            //{"city_code":"","country_code":"1","fansnum":69,"head":"http://app.qlogo.cn/mbloghead/a024e15a9310b3f5179c","idolnum":35,"isidol":true,"isvip":0,"location":"δ֪","name":"willyjl","nick":"�����","openid":"","province_code":"31","tag":null,"tweet":[{"from":"��Ѷ΢��","id":"73020113642003","text":"ƾ�ҵ���ɫ����Ȼֻ�轻��ô��˰�գ�����������˰�վ֡����������������Ҳ��ҹ�ά������������Ҫ�ɶ���#����˰#��http://url.cn/3YFsGb ","timestamp":1310006202}]},
            //{"city_code":"","country_code":"1","fansnum":12,"head":"http://app.qlogo.cn/mbloghead/7d5c58b2cdd733963f32","idolnum":37,"isidol":true,"isvip":0,"location":"δ֪","name":"nina20101012","nick":"����","openid":"","province_code":"31","tag":null,"tweet":[{"from":"��Ѷ΢��","id":"78580001467265","text":"���쿴�����ˣ��Ǹ������õ�СŮ����̫�����ˣ������ӱ�4������ѹ��ȥ���������Ա�����ô���صĺ���ģ�Ϊʲô·�˶���ôĮ�ӣ���������������̫�����ˣ��������ǣ����б�Ӧ��","timestamp":1318943847}]},
            //{"city_code":"1","country_code":"1","fansnum":168,"head":"http://app.qlogo.cn/mbloghead/57875e17a4bce1fbb702","idolnum":284,"isidol":true,"isvip":0,"location":"δ֪","name":"killmyleon","nick":"����","openid":"","province_code":"41","tag":[{"id":"3296752990863134558","name":"����ING"},{"id":"3428083006598920604","name":"����"},{"id":"8486326060299489387","name":"а��"},{"id":"9393533470027694381","name":"����Ӱ"},{"id":"12093106934468067613","name":"ubuntu"},{"id":"13717765964038710262","name":"Ŭ��ING"},{"id":"14438978826104116590","name":"debian"},{"id":"14847358170114098914","name":"linux"},{"id":"16469568389185112236","name":"����լ"}],"tweet":[{"from":"QQǩ��","id":"52611047553060","text":"��־���ʤ����","timestamp":1322091149}]},
            //{"city_code":"","country_code":"","fansnum":229,"head":"http://app.qlogo.cn/mbloghead/ad7b87d35771ebd0bcd0","idolnum":125,"isidol":true,"isvip":0,"location":"","name":"iamyanghua","nick":"�","openid":"","province_code":"","tag":[{"id":"77846971042806542","name":"����"},{"id":"4762518206506141882","name":"��Ϸ"}],"tweet":[{"from":"","id":"0","text":"","timestamp":0}]},
            //{"city_code":"","country_code":"","fansnum":1253,"head":"http://app.qlogo.cn/mbloghead/21663a093b620c1150d8","idolnum":6,"isidol":true,"isvip":0,"location":"δ֪","name":"MEIZU_SH","nick":"����_�Ϻ�","openid":"","province_code":"","tag":[{"id":"3314035123865995316","name":"������һ��콢��"},{"id":"4766081530055180763","name":"�����Ϻ�"},{"id":"5840235084298680107","name":"�����ֻ�"},{"id":"9912118391828821150","name":"MEIZU"},{"id":"13082348043929536259","name":"����"},{"id":"13609997272425797546","name":"����ר����"},{"id":"14986941257412550705","name":"M9"},{"id":"15053646515186054983","name":"�����콢��"},{"id":"15236338144929393964","name":"�Ϻ�����"}],"tweet":[{"from":"��Ѷ΢��","id":"19218047161270","text":"#MX������Ϣ#����MX������Ƶ http://url.cn/3MYnA4   ���һ��Ϊ�� http://url.cn/3yjgiY ","timestamp":1323313998}]},
            //{"city_code":"","country_code":"1","fansnum":3160,"head":"http://app.qlogo.cn/mbloghead/09694c818d98c2b440a6","idolnum":10,"isidol":true,"isvip":0,"location":"δ֪","name":"lovemeizu","nick":"������","openid":"","province_code":"31","tag":[{"id":"399381935516400475","name":"m9"},{"id":"1433216300070607690","name":"80��"},{"id":"7120965338382079114","name":"��ͷ�����򵽱���"},{"id":"12149833612181773251","name":"android"},{"id":"13082348043929536259","name":"����"},{"id":"14193650147949730220","name":"����"},{"id":"14743408212463651736","name":"m8"},{"id":"17382413701540968475","name":"û������"}],"tweet":[{"from":"��Ѷ΢��","id":"38016132218810","text":"����M9�ֻ�����ʱ���ܻ���ÿ������88Ԫ��186�ײ͹�ѡ�񡣺�Լ���۸��������һ����ͬΪ8GB��2499Ԫ��16GB��2699Ԫ��  �������� J.Wong �״��ۼ�M9����ͨ��Լ�ײͷ�������Ϣ��ͬʱ��¶16GB��M9�ļ۸�Ϊ2699Ԫ�� ������Ŀǰ�ײͷ�����δ�������ᵽ��������к����ײ͵Ļ���ʱ����ֱ����ר���������","timestamp":1290763523}]},
            //{"city_code":"","country_code":"1","fansnum":9362,"head":"http://app.qlogo.cn/mbloghead/32f1c38a5d2ef62b918e","idolnum":0,"isidol":true,"isvip":0,"location":"δ֪","name":"meizutech","nick":"������Ѷ","openid":"","province_code":"31","tag":[{"id":"77846971042806542","name":"����"},{"id":"595526330566555944","name":"����̳"},{"id":"797311093297244621","name":"��ɽ"},{"id":"3428083006598920604","name":"����"},{"id":"7632343735443733612","name":"������"},{"id":"9393533470027694381","name":"����Ӱ"},{"id":"13082348043929536259","name":"����"},{"id":"14728618028410374988","name":"��Ӱ"},{"id":"14986941257412550705","name":"M9"},{"id":"17168216440080056340","name":"����"}],"tweet":[{"from":"��Ѷ΢��","id":"71120012656231","text":"����ı�ֽ���ŵİ�Ȩ�Ѿͼ������𣬲�֪����ҵ��ȨҪ����Ǯ���϶������ˡ�","timestamp":1323324978}]},
            //{"city_code":"1","country_code":"1","fansnum":18,"head":"http://app.qlogo.cn/mbloghead/029e58a18b7fc410a238","idolnum":80,"isidol":true,"isvip":0,"location":"δ֪","name":"z580019","nick":"zxm","openid":"","province_code":"41","tag":null,"tweet":[{"from":"QQ�ռ�˵˵","id":"220052593834","text":"[em]e300[/em]v","timestamp":1323041442}]}],
            //"timestamp":1323327658},"errcode":0,"msg":"ok","ret":0}
            return list;
        }
        /// <summary>
        /// ͬ����Ϣ
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <param name="text">��Ϣ</param>
        public void SendText(string accessToken, string accessSecret, string text) {
            List<UrlParameter> param = new List<UrlParameter>();
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_token", accessToken));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Add(new UrlParameter("format", "json"));
            param.Add(new UrlParameter("content", text.SubString(270, "").UrlUpperEncode()));
            //param.Add(new UrlParameter("clientip", "127.0.0.1"));
            //param.Add(new UrlParameter("jing", ""));
            //param.Add(new UrlParameter("wei", ""));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign = new StringBuilder().Append("POST&")
                .Append(add.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, accessSecret, sbSign.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());
            HttpHelper.SendPost(add, OAuthCommon.GetUrlParameter(param));
        }
    }
}
