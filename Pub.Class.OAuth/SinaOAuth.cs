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
using System.Net;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// sina ��Ȩ��¼
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SinaOAuth : IOAuth {
        /// <summary>
        /// request_token
        /// </summary>
        public static readonly string request_token = "http://api.t.sina.com.cn/oauth/request_token";
        /// <summary>
        /// authorize
        /// </summary>
        public static readonly string authorize = "http://api.t.sina.com.cn/oauth/authorize";
        /// <summary>
        /// access_token
        /// </summary>
        public static readonly string access_token = "http://api.t.sina.com.cn/oauth/access_token";
        /// <summary>
        /// user_info
        /// </summary>
        public static readonly string user_info = "http://api.t.sina.com.cn/account/verify_credentials.xml";
        /// <summary>
        /// friends_list
        /// </summary>
        public static readonly string friends_list = "http://api.t.sina.com.cn/statuses/friends.json";
        /// <summary>
        /// add
        /// </summary>
        public static readonly string add = "http://api.t.sina.com.cn/statuses/update.json";
        /// <summary>
        /// sina app ������Ϣ
        /// </summary>
        public static readonly ConfigInfo config = OAuthConfig.GetConfigInfo(OAuthEnum.sina);
        /// <summary>
        /// ȡ��Ȩ��¼URL
        /// </summary>
        /// <returns>��¼URL</returns>
        public string GetAuthUrl() {
            List<UrlParameter> param = new List<UrlParameter>();
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

            int intOTS = data.IndexOf("oauth_token=");
            int intOTSS = data.IndexOf("&oauth_token_secret=");
            string oauth_token = data.Substring(intOTS + 12, intOTSS - (intOTS + 12));
            string oauth_token_secret = data.Substring((intOTSS + 20), data.Length - (intOTSS + 20));
            Session2.Set("oauth_token", oauth_token);
            Session2.Set("oauth_token_secret", oauth_token_secret);
            return (authorize + "?oauth_token=" + oauth_token + "&oauth_callback=" + config.RedirectUrl);
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
            
            int intOTS = data.IndexOf("oauth_token=");
            int intOTSS = data.IndexOf("&oauth_token_secret=");
            int intUser = data.IndexOf("&user_id=");
            user.Token = data.Substring(intOTS + 12, intOTSS - (intOTS + 12));
            user.Secret = data.Substring((intOTSS + 20), intUser - (intOTSS + 20));
            user.UserID = data.Substring((intUser + 9), data.Length - (intUser + 9));

            param.Clear();
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_token", user.Token));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign2 = new StringBuilder().Append("GET&")
                .Append(user_info.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, user.Secret, sbSign2.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());
            data = HttpHelper.SendGet(new StringBuilder().Append(user_info).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());
            
            user.Name = data.GetMatchingValues("<name>(.+?)</name>", "<name>", "</name>").FirstOrDefault() ?? "";
            user.Header = data.GetMatchingValues("<profile_image_url>(.+?)</profile_image_url>", "<profile_image_url>", "</profile_image_url>").FirstOrDefault() ?? "";
            user.Sex = (data.GetMatchingValues("<gender>(.+?)</gender>", "<gender>", "</gender>").FirstOrDefault() ?? "").ToLower().Equals("m") ? 1 : 0;
            user.Address = data.GetMatchingValues("<location>(.+?)</location>", "<location>", "</location>").FirstOrDefault() ?? "";

            //Msg.Write(GetFriendsInfo(user.Token, user.Secret).ToJson());
            //SendText(user.Token, user.Secret, "��������2");
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
                param.Add(new UrlParameter("source", config.AppKey));
                param.Add(new UrlParameter("count", count));
                param.Add(new UrlParameter("cursor", count * (page-1)));
                param.Sort(new UrlParameterCompre());

                StringBuilder sbSign = new StringBuilder().Append("GET&")
                    .Append(friends_list.UrlEncode2())
                    .Append("&")
                    .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

                param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, accessSecret, sbSign.ToString()).UrlEncode2()));
                param.Sort(new UrlParameterCompre());
                string data = "";
                try {
                    data = HttpHelper.SendGet(new StringBuilder().Append(friends_list).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());
                    data = data.Substring(1, data.Length - 2);
                } catch {}
                IList<string> userlist = data.GetMatchingValues("{\"id\":(.+?)}}", "{", "}}");

                foreach (string info in userlist) {
                    UserInfo user = new UserInfo();
                    user.UserID = info.GetMatchingValues("\"id\":(.+?),", "\"id\":", ",").FirstOrDefault() ?? "";
                    user.Email = info.GetMatchingValues("\"email\":\"(.+?)\"", "\"email\":\"", "\"").FirstOrDefault() ?? "";
                    user.Name = info.GetMatchingValues("\"name\":\"(.+?)\"", "\"name\":\"", "\"").FirstOrDefault() ?? "";
                    user.Sex = (info.GetMatchingValues("\"gender\":\"(.+?)\"", "\"gender\":\"", "\"").FirstOrDefault() ?? "") == "m" ? 1 : 0;
                    user.Address = info.GetMatchingValues("\"location\":\"(.+?)\"", "\"location\":\"", "\"").FirstOrDefault() ?? "";
                    user.Header = info.GetMatchingValues("\"profile_image_url\":\"(.+?)\"", "\"profile_image_url\":\"", "\"").FirstOrDefault() ?? "";
                    list.Add(user);
                }

                if (userlist.IsNull() || userlist.Count == 0) isTrue = false;
                page++;
            };

            //{"users":[
            //{"id":1972885037,"screen_name":"","name":"","province":"11","city":"8","location":"","description":"","url":"","profile_image_url":"","domain":"diandianteam","gender":"m","followers_count":74327,"friends_count":69,"statuses_count":430,"favourites_count":1,"created_at":"Wed Feb 16 00:00:00 +0800 2011","following":false,"allow_all_act_msg":true,"geo_enabled":false,"verified":true,"remark":"","status":{"created_at":"Thu Dec 08 13:35:46 +0800 2011","id":3388322699728824,"text":"�������ֵܲ�Ʒ������Ŷ[�õ���]","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388322699728824","retweeted_status":{"created_at":"Wed Dec 07 15:46:22 +0800 2011","id":3387993178396122,"text":"��������iPhone/iTouch/iPad�ϵ��������Ϸ��ʱ��ͬ���ƾ���һ�������԰����ҵ����к�����û���Ҫ����ķ���С���֡����ڵ�½AppStore������ֱ������ͬ��������iTunes���ص�ַ��http://t.cn/SqiARV","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww2.sinaimg.cn/thumbnail/77dc26f5gw1dnu2qpro98j.jpg","bmiddle_pic":"http://ww2.sinaimg.cn/bmiddle/77dc26f5gw1dnu2qpro98j.jpg","original_pic":"http://ww2.sinaimg.cn/large/77dc26f5gw1dnu2qpro98j.jpg","geo":null,"mid":"3387993178396122","user":{"id":2010916597,"screen_name":"ͬ����","name":"ͬ����","province":"35","city":"2","location":"���� ����","description":"һվʽ��������ء���װ������Ϸ��Ӧ�ã�������iPhone/iPad/iPod Touch �û�����ʹ�õ����Ӧ���̵ꡣ","url":"http://tui.tongbu.com","profile_image_url":"http://tp2.sinaimg.cn/2010916597/50/5617123876/0","domain":"tongbutui","gender":"f","followers_count":7341,"friends_count":39,"statuses_count":566,"favourites_count":3,"created_at":"Mon Mar 07 00:00:00 +0800 2011","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":true}}}},
            //{"id":2493118952,"screen_name":"������","name":"������","province":"33","city":"1","location":"�㽭 ����","description":"�������������ռ���������������ϲ������� http://www.huaban.com/","url":"http://www.huaban.com/","profile_image_url":"http://tp1.sinaimg.cn/2493118952/50/5615173111/0","domain":"huabanwang","gender":"f","followers_count":14452,"friends_count":341,"statuses_count":287,"favourites_count":18,"created_at":"Tue Oct 25 00:00:00 +0800 2011","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":true,"remark":"","status":{"created_at":"Fri Dec 09 09:26:55 +0800 2011","id":3388622462153735,"text":"�������Ȥζ�Ĳɼ������ɰ������������ձ�������һ��GARBAGE BAG ART WORK������������Ʒ�ƻ���������ƹ�����MAQ��������ͼʹ�ý�ͷ��������������ø�Ϊ���ۣ��ó��з羰�������á�������ȫ������Σ���Ӧ�ô���С��ϸ������http://t.cn/SqDsVw [����]","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388622462153735","retweeted_status":{"created_at":"Fri Dec 09 08:59:12 +0800 2011","id":3388615487373188,"text":"���ɰ������������������ձ��Ľ�ͷ�����������ЩƯ���Ĵ��ӣ���϶��������˭���İ���Щ��Ȥ�Ĵ������ⶪ�ڽ�ͷ�أ���ʵ�����ձ�������һ��GARBAGE BAG ART WORK������������Ʒ�ƻ���������ƹ�����MAQ��������ͼ... http://t.cn/SqDsVw  �������� @��������","source":"<a href=\"http://huaban.com/\" rel=\"nofollow\">������</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww3.sinaimg.cn/thumbnail/67f7a5a4jw1dnw282j5mej.jpg","bmiddle_pic":"http://ww3.sinaimg.cn/bmiddle/67f7a5a4jw1dnw282j5mej.jpg","original_pic":"http://ww3.sinaimg.cn/large/67f7a5a4jw1dnw282j5mej.jpg","geo":null,"mid":"3388615487373188","user":{"id":1744283044,"screen_name":"����С��","name":"����С��","province":"33","city":"1","location":"�㽭 ����","description":"������ʺ�����ص��","url":"","profile_image_url":"http://tp1.sinaimg.cn/1744283044/50/1289142138/1","domain":"jf882736","gender":"m","followers_count":758,"friends_count":228,"statuses_count":1173,"favourites_count":11,"created_at":"Thu May 20 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false}}}},
            //{"id":1662047260,"screen_name":"SinaAppEngine","name":"SinaAppEngine","province":"11","city":"8","location":"���� ������","description":"Sina App Engineרע���ṩ��Ʒ�ʵ�Ӧ���Ʒ���.�����Է������ǵ���վ�˽������Ϣ http://sae.sina.com.cn","url":"http://sae.sina.com.cn","profile_image_url":"http://tp1.sinaimg.cn/1662047260/50/1258613353/1","domain":"saet","gender":"m","followers_count":111018,"friends_count":114,"statuses_count":1867,"favourites_count":15,"created_at":"Thu Nov 19 00:00:00 +0800 2009","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":true,"remark":"","status":{"created_at":"Fri Dec 09 10:02:38 +0800 2011","id":3388631445927062,"text":"��ϲ��#���ϻ#�滶�֣�","source":"<a href=\"http://event.weibo.com/\" rel=\"nofollow\">΢�</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388631445927062","retweeted_status":{"created_at":"Thu Dec 08 16:25:46 +0800 2011","id":3388365481554324,"text":"O(��_��)O#���ϻ#��������!http://t.cn/Squlm0�����ľ��#�н��#һ��ǧ������ɣ�Ҳȱ��#ͬ�ǻ#���������ο��ܣ�But!TA�п��������ʵ��ģ�TA���Դ��⡢������С�TA����#��ͨ ���� 2X����#�������ʣ����˸�̾#�������Ҿ�XX��#��TA������΢��ҵλ�������Ϸhttp://t.cn/Sqn9hQ","source":"<a href=\"http://event.weibo.com/\" rel=\"nofollow\">΢�</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww4.sinaimg.cn/thumbnail/6f20c747tw1dnv9826m22j.jpg","bmiddle_pic":"http://ww4.sinaimg.cn/bmiddle/6f20c747tw1dnv9826m22j.jpg","original_pic":"http://ww4.sinaimg.cn/large/6f20c747tw1dnv9826m22j.jpg","geo":null,"mid":"3388365481554324","user":{"id":1864419143,"screen_name":"�С�ӳ�","name":"�С�ӳ�","province":"11","city":"8","location":"���� ������","description":"����΢��΢�ƽ̨�ٷ�΢���� ���׻�Ƽ� ��ϲ������    <br />\n��һ������ 9��30-18��30 Ϊ�����ɽ��<br />\n         <br />\n�����\\��","url":"http://event.weibo.com","profile_image_url":"http://tp4.sinaimg.cn/1864419143/50/5601946761/0","domain":"event","gender":"f","followers_count":877553,"friends_count":394,"statuses_count":2439,"favourites_count":8,"created_at":"Tue Nov 16 00:00:00 +0800 2010","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":true},"annotations":[{"source":{"id":"294315","appid":"38","name":"#���ϻ#�������ߣ�ת��Ӯ������Ʒ","title":"#���ϻ#������...","url":"http://event.weibo.com/294315"}}]}}},
            //{"id":1579058951,"screen_name":"�μ���ѵ-�μ��","name":"�μ���ѵ-�μ��","province":"11","city":"2","location":"���� ������","description":"����������IT������һ��ˮ׼��רҵ��ЧPPT����ٿμ���","url":"http://blog.sina.com.cn/kejianttt","profile_image_url":"http://tp4.sinaimg.cn/1579058951/50/5609489542/0","domain":"kejianttt","gender":"f","followers_count":1081,"friends_count":318,"statuses_count":206,"favourites_count":26,"created_at":"Sat Nov 07 00:00:00 +0800 2009","following":false,"allow_all_act_msg":true,"geo_enabled":false,"verified":false,"remark":"","status":{"created_at":"Fri Dec 09 08:53:52 +0800 2011","id":3388614145058479,"text":"#JJѧϰ#�����γ���Ч�ԵĹؼ�֮�ģ��γ��Ƿ���ѧԱչʾ�����̶ȣ� ����ѧԱ�Կγ����ݵ����̶ȣ���������ѧԱ����ѧ����������֪ʶ����������ѡ������ж���ֻ����ʾһ�����Ƿ�֪����ĳ����ʵ��Ҫ����ѧԱ����ѧ֪ʶ�����̶ȣ�Ӧ���ǿ����Ƿ��ܰ���Щ��ʵ�����ھ��ߡ�(�μ��������Articulate)","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388614145058479"}},{"id":1175961930,"screen_name":"��С��ͯЬ","name":"��С��ͯЬ","province":"31","city":"15","location":"�Ϻ� �ֶ�����","description":"������һ�ˣ�������������","url":"http://blog.sina.com.cn/yamamototaro","profile_image_url":"http://tp3.sinaimg.cn/1175961930/50/5607261690/1","domain":"yamamototaro","gender":"m","followers_count":879,"friends_count":182,"statuses_count":6460,"favourites_count":109,"created_at":"Wed Mar 31 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Thu Dec 08 22:13:17 +0800 2011","id":3388452935175529,"text":"ת��΢����","source":"<a href=\"http://weibo.com/mobile/iphone.php\" rel=\"nofollow\">iPhone�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388452935175529","retweeted_status":{"created_at":"Thu Dec 08 21:43:10 +0800 2011","id":3388445355582747,"text":"�е�������Դ�ĵ��⣬��Ҫ���¼����󣬲��ܿ�ʼ�����������е��������С�ĵ��⣬�ų�һ�ڣ��ͳ����ˡ��е������������Ȧ����һ�߇���������һ������Ŀ������������ǿյġ��������� ���� - ��δ֪���˵İ������ ��...........................( ͼ * ֣ά ��Ʒ )","source":"<a href=\"http://weibo.com/mobile/iphone.php\" rel=\"nofollow\">iPhone�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww1.sinaimg.cn/thumbnail/4c69db7djw1dnvionwxohj.jpg","bmiddle_pic":"http://ww1.sinaimg.cn/bmiddle/4c69db7djw1dnvionwxohj.jpg","original_pic":"http://ww1.sinaimg.cn/large/4c69db7djw1dnvionwxohj.jpg","geo":null,"mid":"3388445355582747","user":{"id":1282005885,"screen_name":"�̿���","name":"�̿���","province":"71","city":"1000","location":"̨��","description":"** �������s����, ՈǢ���o��˾: ying@hte888.com // ( ��������FAX )+86-10-65309417 // ( ̨������FAX )+886-2-27523638","url":"http://blog.sina.com.cn/caikangyong","profile_image_url":"http://tp2.sinaimg.cn/1282005885/50/5617303476/1","domain":"caikangyong","gender":"m","followers_count":12115806,"friends_count":413,"statuses_count":489,"favourites_count":1577,"created_at":"Fri Aug 28 00:00:00 +0800 2009","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":true},"annotations":[{"server_ip":"10.73.19.66"}]},"annotations":[]}},
            //{"id":1712092715,"screen_name":"eLearningʵʩ","name":"eLearningʵʩ","province":"31","city":"1000","location":"�Ϻ�","description":"�Ȱ�e-Learning����עe-Learning��״�ͷ�չ���ƣ�רע����ҵe-Learningʵʩ��","url":"","profile_image_url":"http://tp4.sinaimg.cn/1712092715/50/1300094129/0","domain":"sumanelearning","gender":"f","followers_count":538,"friends_count":173,"statuses_count":73,"favourites_count":11,"created_at":"Wed Mar 31 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Sat Aug 13 17:49:25 +0800 2011","id":3345987150202697,"text":"ÿ�����ڲ�ͬ�ĳ��ϣ���Ҫ���ݲ�ͬ�Ľ�ɫ�����ֳ��ϻ������ɫ���ǲ�����ġ������ڵ��¡����ڵ���ȫ��Ͷ�룬�ݺ��ҵĽ�ɫ��","source":"<a href=\"http://weibo.com/mobile/wap.php\" rel=\"nofollow\">����΢���ֻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3345987150202697","retweeted_status":{"created_at":"Sat Aug 13 16:41:33 +0800 2011","id":3345970070820733,"text":"��һ�����ڿ��ȹݽ��ܼ��ߵĲɷã���̸��̸������ѧ��Ӱ�Ӹı࣬��һ���Ӳɷý������������������У����������˻ؼ�������ŷ��ɣ��[����]","source":"<a href=\"http://weibo.com/mobile/iphone.php\" rel=\"nofollow\">iPhone�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3345970070820733","user":{"id":1225419417,"screen_name":"����˼��","name":"����˼��","province":"42","city":"1","location":"���� �人","description":"��������������ϸ塢��Ъˢ�������졢��װ���Ļ���Ʒζ�����@��������д�ţ�feiwsc@sohu.com","url":"http://www.feiwosicun.net/bbs/index.php","profile_image_url":"http://tp2.sinaimg.cn/1225419417/50/1279875696/0","domain":"fwsc","gender":"f","followers_count":388309,"friends_count":163,"statuses_count":3350,"favourites_count":6,"created_at":"Fri Aug 28 00:00:00 +0800 2009","following":false,"allow_all_act_msg":false,"geo_enabled":false,"verified":true},"annotations":[{"server_ip":"10.73.19.140"}]},"annotations":[]}},
            //{"id":2030979823,"screen_name":"�ֺ�lloyd","name":"�ֺ�lloyd","province":"31","city":"1000","location":"�Ϻ�","description":"","url":"","profile_image_url":"http://tp4.sinaimg.cn/2030979823/50/5602042778/1","domain":"","gender":"m","followers_count":47,"friends_count":39,"statuses_count":21,"favourites_count":11,"created_at":"Thu Mar 17 00:00:00 +0800 2011","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Tue Nov 29 12:00:29 +0800 2011","id":3385037228465654,"text":"�@�����N�������30�w�ݽz����Ҫ���˵ģ�����[ŭ��][ŭ��][ŭ��]","source":"<a href=\"http://weibo.com/mobile/android.php\" rel=\"nofollow\">Android�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3385037228465654","retweeted_status":{"created_at":"Tue Nov 29 10:30:11 +0800 2011","id":3385014504838578,"text":"���������չ�˾�ͻ����й�ά�޺�����30���ݶ���̫�����龪�ˣ��������չ�˾һ�ܿտ�A340�ͻ����й�����ά�޺󣬼������У�������ŷ��ַɻ���һ�鱣�����Ͼ�Ȼ���˽�30����˿�����̡��й����족֮�󣬡��й�ά�ޡ�ҲҪ���������ˣ�http://t.cn/SUrBIn","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww4.sinaimg.cn/thumbnail/71ced708tw1dnkknmmmxfj.jpg","bmiddle_pic":"http://ww4.sinaimg.cn/bmiddle/71ced708tw1dnkknmmmxfj.jpg","original_pic":"http://ww4.sinaimg.cn/large/71ced708tw1dnkknmmmxfj.jpg","geo":null,"mid":"3385014504838578","user":{"id":1909380872,"screen_name":"��ҵ���ǿ�","name":"��ҵ���ǿ�","province":"11","city":"1000","location":"����","description":"΢�������Ӱ�������̽羫Ӣ�ۼ��أ���ӭ��ע����","url":"http://weibo.com/jingcaimingren","profile_image_url":"http://tp1.sinaimg.cn/1909380872/50/5617266792/1","domain":"nannvqushi","gender":"m","followers_count":184194,"friends_count":378,"statuses_count":5545,"favourites_count":1591,"created_at":"Mon Dec 27 00:00:00 +0800 2010","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":false}},"annotations":[]}},
            //{"id":1842500341,"screen_name":"��̬������","name":"��̬������","province":"31","city":"1000","location":"�Ϻ�","description":"�ź������Ծܾ�ͬ�����۵����壡","url":"","profile_image_url":"http://tp2.sinaimg.cn/1842500341/50/5615791638/1","domain":"","gender":"m","followers_count":98,"friends_count":74,"statuses_count":541,"favourites_count":2,"created_at":"Sat Oct 30 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Thu Dec 08 09:18:24 +0800 2011","id":3388257929528921,"text":"�й���ѧ�ĸ�����ʵ��˼�磬�к�ǿ���߼�������ֻ��Ч��������ǡǡ�෴����Ч�������������ٲ����Чһ�����߼���������Ϊ�㿴��͸����Ϊ����Ҫ�����Ǳ�ĩ���á���ѵ����������ѵҵ�� //@ؼ�´��:// @�Կ��� : �����������࣬������ˮ����֮��������֮��ζ��","source":"<a href=\"http://weibo.com/mobile/iphone.php\" rel=\"nofollow\">iPhone�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388257929528921","retweeted_status":{"created_at":"Wed Dec 07 23:42:57 +0800 2011","id":3388113113705520,"text":"�ж�ѧϰ�߻�ʦ��Ҫ����ҵ�Ļ��г�ֵ���ʶ����⣬������ݵ���е�������뼼���У�����ѧϰ��������ʱ���̡���ϵͳ��гǰ���£���Ч���е������Ҫ��","source":"<a href=\"http://weibo.com/mobile/ipad.php\" rel=\"nofollow\">iPad�ͻ���</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388113113705520","user":{"id":1738470153,"screen_name":"����nichole","name":"����nichole","province":"44","city":"3","location":"�㶫 ����","description":"������ѵ ��ѵʦ��ѵ ��ѵϵͳ","url":"http://blog.sina.com.cn/u/1738470153","profile_image_url":"http://tp2.sinaimg.cn/1738470153/50/5616159177/0","domain":"sznichole","gender":"f","followers_count":906,"friends_count":337,"statuses_count":1280,"favourites_count":123,"created_at":"Tue Jul 13 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false}},"annotations":[]}},
            //{"id":1890959170,"screen_name":"������","name":"������","province":"31","city":"1000","location":"�Ϻ�","description":"","url":"","profile_image_url":"http://tp3.sinaimg.cn/1890959170/50/5613749110/0","domain":"yvonne001","gender":"f","followers_count":90,"friends_count":98,"statuses_count":712,"favourites_count":26,"created_at":"Thu Dec 09 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Thu Dec 08 17:50:54 +0800 2011","id":3388386905931032,"text":"//@��С��ͯЬ: @Ҽ����B","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388386905931032","retweeted_status":{"created_at":"Mon Nov 28 15:35:05 +0800 2011","id":3384728843254125,"text":"#YouTube������#��׹�����£������µ��ǡ���������˵����Ļ�����[����] ���������ϵĿս㡣������[����] http://t.cn/S4GtnR ���ǵ��������ǵ���ѽ����[ץ��]","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww3.sinaimg.cn/thumbnail/83fae389tw1dnjntnk65kj.jpg","bmiddle_pic":"http://ww3.sinaimg.cn/bmiddle/83fae389tw1dnjntnk65kj.jpg","original_pic":"http://ww3.sinaimg.cn/large/83fae389tw1dnjntnk65kj.jpg","geo":null,"mid":"3384728843254125","user":{"id":2214257545,"screen_name":"YouTube��ѡ","name":"YouTube��ѡ","province":"400","city":"1","location":"���� ����","description":"����Ҫˢ΢��������ҲҪˢ΢������Ϊһ�����˹�������Ҫ���ǲ��º�з��ǽ��ǽ�ⶼ�Ǹ��ƣ�����YouTube��ѡ������weibo.com","url":"http://weibo.com/youtube","profile_image_url":"http://tp2.sinaimg.cn/2214257545/50/5604558022/0","domain":"youtube","gender":"f","followers_count":326225,"friends_count":327,"statuses_count":2312,"favourites_count":9,"created_at":"Sat Jul 02 00:00:00 +0800 2011","following":false,"allow_all_act_msg":true,"geo_enabled":true,"verified":false}}}},
            //{"id":1688983163,"screen_name":"keylogic����","name":"keylogic����","province":"11","city":"1","location":"���� ������","description":"KeyLogic��˾��ʼ�ˣ����С��������ˡ���ʹ���͡�רҵ���塱�ļ�ֵ�۴�ҵ�����˲š�ϲս�ԡ������͵�������������","url":"","profile_image_url":"http://tp4.sinaimg.cn/1688983163/50/5617696588/1","domain":"wangchengkeylogic","gender":"m","followers_count":1714,"friends_count":458,"statuses_count":969,"favourites_count":1,"created_at":"Tue Feb 02 00:00:00 +0800 2010","following":false,"allow_all_act_msg":false,"geo_enabled":true,"verified":false,"remark":"","status":{"created_at":"Thu Dec 08 23:45:38 +0800 2011","id":3388476173068478,"text":"Խ����ʱԽ��Ҫ������ʱ��������:)","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","geo":null,"mid":"3388476173068478","retweeted_status":{"created_at":"Thu Dec 08 23:44:22 +0800 2011","id":3388475858817826,"text":"������æʱ�����׳������ң��������¸��ҡ��ع�ʦ�� @��Ⱥ��ʦ ������ϼ��΢�����������鵽�ľ����������Խ����ʱԽ��Ҫ������ʱ�������塣ͬʱ����ʱ�����������������ʱ����������ص���˾����������˼һ�£����徻��ࡣ����ȫ���Ը���������Ե����Թ�޻ڣ�","source":"<a href=\"http://weibo.com\" rel=\"nofollow\">����΢��</a>","favorited":false,"truncated":false,"in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","thumbnail_pic":"http://ww3.sinaimg.cn/thumbnail/68583a48gw1dnvm4bzfd3j.jpg","bmiddle_pic":"http://ww3.sinaimg.cn/bmiddle/68583a48gw1dnvm4bzfd3j.jpg","original_pic":"http://ww3.sinaimg.cn/large/68583a48gw1dnvm4bzfd3j.jpg","geo":null,"mid":"3388475858817826","user":{"id":1750612552,"screen_name":"������Ե","name":"������Ե","province":"33","city":"1","location":"�㽭 ����","description":"�����@��Ⱥ��ʦ�����£���������Ժ��ѧ���εڣ���Ե���ң�������ã�����������ѧ���ϣ�����ʮ�ꡢ����ͬ�ۣ�----�Ա���Ե��(��褡����;�ʿ��","url":"http://blog.sina.com.cn/putichanyuan","profile_image_url":"http://tp1.sinaimg.cn/1750612552/50/1279901586/1","domain":"putichanyuan","gender":"m","followers_count":11522,"friends_count":473,"statuses_count":1331,"favourites_count":30,"created_at":"Fri Jun 04 00:00:00 +0800 2010","following":false,"allow_all_act_msg":true,"geo_enabled":false,"verified":true}}}}
            //],"next_cursor":10,"previous_cursor":0}
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
            param.Add(new UrlParameter("source", config.AppKey));
            param.Add(new UrlParameter("status", text.SubString(270, "").UrlUpperEncode()));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign = new StringBuilder().Append("POST&")
                .Append(add.UrlEncode2())
                .Append("&")
                .Append(OAuthCommon.GetUrlParameter(param).UrlEncode2());

            param.Add(new UrlParameter("oauth_signature", OAuthCommon.GetHMACSHA1(config.AppSecret, accessSecret, sbSign.ToString()).UrlEncode2()));
            param.Sort(new UrlParameterCompre());
            HttpHelper.SendPost(add, OAuthCommon.GetUrlParameter(param), "application/x-www-form-urlencoded");
        }
    }
}
