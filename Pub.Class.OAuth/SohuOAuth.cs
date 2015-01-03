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
    /// Sohu ��Ȩ��¼
    /// 
    /// �޸ļ�¼
    ///     2011.12.02 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SohuOAuth : IOAuth {
        /// <summary>
        /// request_token
        /// </summary>
        public static readonly string request_token = "http://api.t.sohu.com/oauth/request_token";
        /// <summary>
        /// authorize
        /// </summary>
        public static readonly string authorize = "http://api.t.sohu.com/oauth/authorize";
        /// <summary>
        /// access_token
        /// </summary>
        public static readonly string access_token = "http://api.t.sohu.com/oauth/access_token";
        /// <summary>
        /// user_info
        /// </summary>
        public static readonly string user_info = "http://api.t.sohu.com/account/verify_credentials.json";
        /// <summary>
        /// friends_list
        /// </summary>
        public static readonly string friends_list = "http://api.t.sohu.com/statuses/friends.json";
        /// <summary>
        /// add
        /// </summary>
        public static readonly string add = "http://api.t.sohu.com/statuses/update.json";
        /// <summary>
        /// qq app ������Ϣ
        /// </summary>
        public static readonly ConfigInfo config = OAuthConfig.GetConfigInfo(OAuthEnum.sohu);
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
                .Append(Rfc3986.Encode(request_token))
                .Append("&")
                .Append(Rfc3986.Encode(OAuthCommon.GetUrlParameter(param)));

            param.Add(new UrlParameter("oauth_signature", Rfc3986.Encode(OAuthCommon.GetHMACSHA1(Rfc3986.Encode(config.AppSecret), "", sbSign.ToString()))));
            param.Sort(new UrlParameterCompre());
            string data = HttpHelper.SendGet(new StringBuilder().Append(request_token).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString()) + "&";

            string token = data.GetMatchingValues("oauth_token=(.+?)&", "oauth_token=", "&").FirstOrDefault() ?? "";
            string tokenSecret = data.GetMatchingValues("oauth_token_secret=(.+?)&", "oauth_token_secret=", "&").FirstOrDefault() ?? "";
            Session2.Set("oauth_token", token);
            Session2.Set("oauth_token_secret", tokenSecret);
            return authorize + "?oauth_token=" + token + "&oauth_callback=" + config.RedirectUrl;
        }
        /// <summary>
        /// ȡ��¼�˺���Ϣ
        /// </summary>
        /// <returns>ȡ��¼�˺���Ϣ</returns>
        public UserInfo GetUserInfo() {
            UserInfo user = new UserInfo();

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
                .Append(Rfc3986.Encode(access_token))
                .Append("&")
                .Append(Rfc3986.Encode(OAuthCommon.GetUrlParameter(param)));

            param.Add(new UrlParameter("oauth_signature", Rfc3986.Encode(OAuthCommon.GetHMACSHA1(Rfc3986.Encode(config.AppSecret), Rfc3986.Encode(Session2.Get("oauth_token_secret")), sbSign.ToString()))));
            param.Sort(new UrlParameterCompre());
            string data = HttpHelper.SendGet(new StringBuilder().Append(access_token).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString()) + "&";

            user.Token = data.GetMatchingValues("oauth_token=(.+?)&", "oauth_token=", "&").FirstOrDefault() ?? "";
            user.Secret = data.GetMatchingValues("oauth_token_secret=(.+?)&", "oauth_token_secret=", "&").FirstOrDefault() ?? "";

            param.Clear();
            param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
            param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
            param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
            param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
            param.Add(new UrlParameter("oauth_token", user.Token));
            param.Add(new UrlParameter("oauth_version", "1.0"));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign2 = new StringBuilder().Append("GET&")
                .Append(Rfc3986.Encode(user_info))
                .Append("&")
                .Append(Rfc3986.Encode(OAuthCommon.GetUrlParameter(param)));

            param.Add(new UrlParameter("oauth_signature", Rfc3986.Encode(OAuthCommon.GetHMACSHA1(Rfc3986.Encode(config.AppSecret), Rfc3986.Encode(user.Secret), sbSign2.ToString()))));
            param.Sort(new UrlParameterCompre());
            data = HttpHelper.SendGet(new StringBuilder().Append(user_info).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());

            user.UserID = data.GetMatchingValues("\"id\":\"(.+?)\"", "\"id\":\"", "\"").FirstOrDefault() ?? "";
            user.Email = data.GetMatchingValues("\"email\":\"(.+?)\"", "\"email\":\"", "\"").FirstOrDefault() ?? "";
            user.Name = data.GetMatchingValues("\"screen_name\":\"(.+?)\"", "\"screen_name\":\"", "\"").FirstOrDefault() ?? "";
            user.Sex = (data.GetMatchingValues("\"gender\":\"(.+?)\"", "\"gender\":\"", "\"").FirstOrDefault() ?? "") == "1" ? 1 : 0;
            user.Header = data.GetMatchingValues("\"profile_image_url\":\"(.+?)\"", "\"profile_image_url\":\"", "\"").FirstOrDefault() ?? "";
            user.Address = data.GetMatchingValues("\"location\":\"(.+?)\"", "\"location\":\"", "\"").FirstOrDefault() ?? "";

            //{"id":"268563401","screen_name":"livexy","name":"","location":"�Ϻ���,�����","description":"","url":"","gender":"0",
            //"profile_image_url":"http://s4.cr.itc.cn/img/t/avt_48.jpg","protected":true,"followers_count":1,"profile_background_color":"",
            //"profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":7,
            //"created_at":"Fri Dec 02 13:26:29 +0800 2011","favourites_count":0,"utc_offset":"","time_zone":"","profile_background_image_url":"",
            //"notifications":"","geo_enabled":false,"statuses_count":0,"following":true,"verified":false,"lang":"zh_cn","contributors_enabled":false}
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
            bool isTrue = true; int count = 5; int page = 1;

            while (isTrue) {
                List<UrlParameter> param = new List<UrlParameter>();
                param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
                param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
                param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
                param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
                param.Add(new UrlParameter("oauth_token", accessToken));
                param.Add(new UrlParameter("oauth_version", "1.0"));
                param.Add(new UrlParameter("page", page));
                param.Add(new UrlParameter("count", count));
                param.Sort(new UrlParameterCompre());

                StringBuilder sbSign = new StringBuilder().Append("GET&")
                    .Append(Rfc3986.Encode(friends_list))
                    .Append("&")
                    .Append(Rfc3986.Encode(OAuthCommon.GetUrlParameter(param)));

                param.Add(new UrlParameter("oauth_signature", Rfc3986.Encode(OAuthCommon.GetHMACSHA1(Rfc3986.Encode(config.AppSecret), Rfc3986.Encode(accessSecret), sbSign.ToString()))));
                param.Sort(new UrlParameterCompre());
                string data = "";
                try {
                    data = HttpHelper.SendGet(new StringBuilder().Append(friends_list).Append("?").Append(OAuthCommon.GetUrlParameter(param)).ToString());
                    data = data.Substring(1, data.Length - 2);
                } catch {}
                IList<string> userlist = data.GetMatchingValues("{\"id\":\"(.+?)}}", "{", "}}");

                foreach (string info in userlist) {
                    UserInfo user = new UserInfo();
                    user.UserID = info.GetMatchingValues("\"id\":\"(.+?)\"", "\"id\":\"", "\"").FirstOrDefault() ?? "";
                    user.Email = info.GetMatchingValues("\"email\":\"(.+?)\"", "\"email\":\"", "\"").FirstOrDefault() ?? "";
                    user.Name = info.GetMatchingValues("\"screen_name\":\"(.+?)\"", "\"screen_name\":\"", "\"").FirstOrDefault() ?? "";
                    user.Sex = (info.GetMatchingValues("\"gender\":\"(.+?)\"", "\"gender\":\"", "\"").FirstOrDefault() ?? "") == "1" ? 1 : 0;
                    user.Address = info.GetMatchingValues("\"location\":\"(.+?)\"", "\"location\":\"", "\"").FirstOrDefault() ?? "";
                    user.Header = info.GetMatchingValues("\"profile_image_url\":\"(.+?)\"", "\"profile_image_url\":\"", "\"").FirstOrDefault() ?? "";
                    list.Add(user);
                }

                if (userlist.IsNull() || userlist.Count == 0) isTrue = false;
                page++;
            };

            //"users":[
            //{"id":"8641996","screen_name":"����ͷ��","name":"","location":"������,������","description":"","url":"","gender":"1","profile_image_url":"http://s4.cr.itc.cn/mblog/icon/c7/4b/m_13119583882378.JPG","protected":true,"followers_count":5301900,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":1217,"created_at":"Tue Jun 29 14:35:44 +0800 2010","favourites_count":5,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":18590,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 13:05:40 +0800 2011","id":"2463351713","text":"����³ľ��һŮ��������5����ǿ�Ծ����� ��12��6����11ʱ30�֣��½����б��������Ů���������°�ؼң��ս���С����ͻ���ͽ�ֵ����٣�����5������ǻ����ǻ����ۡ����ȵȶദ��ͱ����β����ѡ����˺��������к����⳵��������ҽԺ�������ѱ�����http://t.itc.cn/LHsfW","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_13/f_5772229398968603.jpg","middle_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_13/m_5772229398968603.jpg","original_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_13/5772229398968603.jpg"}},
            //{"id":"6376033","screen_name":"�Ѻ���Ƶ","name":"","location":"������,-","description":"�Ѻ���Ƶtv.sohu.com ������ÿ���ʱ�����š���Ӱ�����Ӿ硢��¼Ƭ������Ƭ����Ѷ�������ע��","url":"","gender":"1","profile_image_url":"http://s5.cr.itc.cn/mblog/icon/ac/39/m_13083074776832.jpg","protected":true,"followers_count":2540941,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":531,"created_at":"Tue Jun 08 15:16:04 +0800 2010","favourites_count":2,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":6694,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 12:03:09 +0800 2011","id":"2462806849","text":"#�Ѻ���Ƶ΢����#���νࡶ�������㡷MV�ײ����ν�ȫ��ר���У�����ع�ͬʱҲ������������������ϵ������衶�������㡷MV��ʽ�ײ���MV�кν�չ�ֳ����ݼ���ֱ�����ó����뻯�����ݣ�ÿһ��������ÿһ��״̬��������ر�λ������ÿ��Ϸ����һ��ͨ������ http://t.itc.cn/Lu9HL","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/f_8629582797796467.jpg","middle_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/m_8629582797796467.jpg","original_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/8629582797796467.jpg"}},
            //{"id":"1323475","screen_name":"�Ѻ���ѧ","name":"","location":"������,-","description":"��嫵�����+���������+�����ľ���+��ʷ�ĺۼ����Ѻ���ѧƵ����","url":"","gender":"1","profile_image_url":"http://s5.cr.itc.cn/mblog/icon/f7/db/m_12706074122238.jpg","protected":false,"followers_count":2246515,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":17,"created_at":"Wed Apr 07 10:26:38 +0800 2010","favourites_count":0,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":943,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 12:30:02 +0800 2011","id":"2463028113","text":"����ʿ���ְ�ϸ����ɢ������ ����������;������ʿ��ѧ�ҽ��ڷ�����һ�ִٽ�������ɢ��ת�Ƶġ������������ʡ����ɹ�ϸ����������-2��ʵ��֤�����������ֵ����ʵ������ܹ���Ч���ƶ��������Ļ�Ծ�̶ȡ���һ���ֽ���������֢���ƿ���һ��ȫ�µ�;����http://t.itc.cn/L3RsH","source":"ƤƤ����","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/f_8631221896284467.jpg","middle_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/m_8631221896284467.jpg","original_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_12/8631221896284467.jpg"}},
            //{"id":"1333002","screen_name":"�ƾ�ͷ��","name":"","location":"������,-","description":"��ʱ��Ѷ�����ұ��ϡ������������Բƾ��ӽǹ�ע������Ұ��һ����΢���ɣ�","url":"","gender":"1","profile_image_url":"http://s4.cr.itc.cn/mblog/icon/29/c7/m_12931723592217.jpg","protected":false,"followers_count":6491415,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":1458,"created_at":"Thu Apr 08 16:39:36 +0800 2010","favourites_count":4,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":15916,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 12:07:59 +0800 2011","id":"2462845477","text":"�����Ǻ����������Ͼ�һ����ߡ�2009��3�£������Ͼ־��人������һֽ��������Ǻ���ͣ�ɡ�����һ������7���£������������Ͼ���һ�ڵ��񺽵�һ�����١���һ�������ڳ����ˣ����ݰ�������Ժ�ж��Ǻ��հ��ߡ�@���Ǽ��� ���ŷ�����@������ �ƣ����Ǽ��Ž����ߵ��ס�","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_12/f_8629896712993467.jpg","middle_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_12/m_8629896712993467.jpg","original_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_12/8629896712993467.jpg"}},
            //{"id":"205207873","screen_name":"�Ѻ�΢���ٷ���ҥ","name":"","location":"������,������","description":"�Ѻ�΢����ҥ�ٷ��˺�","url":"","gender":"1","profile_image_url":"http://s5.cr.itc.cn/mblog/icon/61/30/m_13153908401706.jpg","protected":true,"followers_count":2725279,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":0,"created_at":"Thu Sep 01 20:17:53 +0800 2011","favourites_count":0,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":71,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 10:41:30 +0800 2011","id":"2462125691","text":"���գ�������������ֲ��Ĺ�����Ů��ʽ��¯��һ�ģ����Ѻ�΢����֤������ҥ�ԡ����գ���λ���ѷ���΢���ơ�������ֲ��Ĺ�����Ů��ʽ��¯������ͼһ�ţ���ϸ�ı�������ν�Ĺ������̡����Ѻ�΢����֤�����ڽ���4�£������ϡ������ؽ�������ҥ�Դ��������������ݾ�������΢���Ϸ������ҥ��������ʾ���ؾ�����δ�ӵ���ؾ��飬��ҥ���еĵ�����·��վ���Ⱦ��д��󣬣��������� http://t.itc.cn/LvAXy�����ڸ�...","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_10/f_5763578358261603.jpg","middle_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_10/m_5763578358261603.jpg","original_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_10/5763578358261603.jpg"}},
            //{"id":"16053","screen_name":"����ͷ��","name":"","location":"������,-","description":"�Ѻ�����Ƶ��΢�����ṩ�������ǰ��ԡ�Ӱ����Ѷ���������������ݣ�ÿ��7��ÿ��24Сʱ���£������ע��","url":"","gender":"1","profile_image_url":"http://s5.cr.itc.cn/mblog/icon/09/50/m_12991200389544.jpg","protected":true,"followers_count":5411328,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":781,"created_at":"Thu Jan 07 17:34:26 +0800 2010","favourites_count":8,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":17283,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 11:20:41 +0800 2011","id":"2462471205","text":"��#Touch of Evil#��ȥ��ŦԼʱ��������Ȳ߻���13λ��Ա�ı��ݿΣ�������̵����ʳ�¯��������а��Ӵ���touch of evil���ҿ�������-Ƥ��(Brad Pitt)Ϸ�¡�#��Ƥͷ#���еĽܿ�-��˹�����������������ﾭ�䣿�ҿ�Ƥ������񾭵��¾���http://t.itc.cn/LmnNU http://t.itc.cn/L3Pvp","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_11/f_5765897143974603.jpg","middle_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_11/m_5765897143974603.jpg","original_pic":"http://s2.t.itc.cn/mblog/pic/201112_9_11/5765897143974603.jpg"}},
            //{"id":"31963453","screen_name":"�Ѻ�΢���ٷ�","name":"","location":"������,������","description":"�Ѻ�΢���ٷ���˺�","url":"","gender":"0","profile_image_url":"http://s5.cr.itc.cn/mblog/icon/6a/98/m_13014576518865.jpg","protected":true,"followers_count":13601146,"profile_background_color":"","profile_text_color":"","profile_link_color":"","profile_sidebar_fill_color":"","profile_sidebar_border_color":"","friends_count":158,"created_at":"Fri Jan 07 16:21:46 +0800 2011","favourites_count":0,"utc_offset":"","time_zone":"","profile_background_image_url":"","notifications":"","geo_enabled":false,"statuses_count":2789,"following":true,"verified":true,"lang":"zh_cn","contributors_enabled":false,"status":{"created_at":"Fri Dec 09 09:55:01 +0800 2011","id":"2461704594","text":"����齱��ÿ��������3:30���Ѻ���Ƶ�� �������ը��http://t.itc.cn/L3ruv �� #�����ը#Ϊ�ؼ���д�±�������Ϊ����Ȥ��3�����ӷ�һ��΢�������ǽ���ѡ10�������߻�����Ѻ�΢���ձ��iPhone��籦����@�Ѻ���Ƶ ����������������飺http://t.itc.cn/L464r","source":"�Ѻ�΢��","favorited":false,"truncated":"","in_reply_to_status_id":"","in_reply_to_user_id":"","in_reply_to_screen_name":"","small_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_9/f_8621637448428467.jpg","middle_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_9/m_8621637448428467.jpg","original_pic":"http://s3.t.itc.cn/mblog/pic/201112_9_9/8621637448428467.jpg"}}
            //],"cursor_id":17283996
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
            param.Add(new UrlParameter("status", text.SubString(270, "").UrlUpperEncode()));
            param.Sort(new UrlParameterCompre());

            StringBuilder sbSign = new StringBuilder().Append("POST&")
                .Append(Rfc3986.Encode(add))
                .Append("&")
                .Append(Rfc3986.Encode(OAuthCommon.GetUrlParameter(param)));

            param.Add(new UrlParameter("oauth_signature", Rfc3986.Encode(OAuthCommon.GetHMACSHA1(Rfc3986.Encode(config.AppSecret), Rfc3986.Encode(accessSecret), sbSign.ToString()))));
            param.Sort(new UrlParameterCompre());
            HttpHelper.SendPost(add, OAuthCommon.GetUrlParameter(param), "application/x-www-form-urlencoded");
        }
    }
}
