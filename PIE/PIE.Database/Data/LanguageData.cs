using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    public class LanguageData
    {
        public static LanguageData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly LanguageData _instance = new LanguageData();
        }

        public static IList<Language> All()
        {
            IList<Language> languages = new List<Language>();
            var af_ZA = new Language()
            {
                Name = "Afrikaans (South Africa)",
                Code = "af-ZA",
                IsActive = true
            };
            languages.Add(af_ZA);

            var sq_AL = new Language()
            {
                Name = "Albanian (Albania)",
                Code = "sq-AL",
                IsActive = true
            };
            languages.Add(sq_AL);

            var am_ET = new Language()
            {
                Name = "Amharic (Ethiopia)",
                Code = "am-ET",
                IsActive = true
            };
            languages.Add(am_ET);

            var ar_SA = new Language()
            {
                Name = "Arabic (Saudi Arabia)",
                Code = "ar-SA",
                IsActive = true
            };
            languages.Add(ar_SA);

            var hy_AM = new Language()
            {
                Name = "Armenian (Armenia)",
                Code = "hy-AM",
                IsActive = true
            };
            languages.Add(hy_AM);

            var as_IN = new Language()
            {
                Name = "Assamese (India)",
                Code = "as-IN",
                IsActive = true
            };
            languages.Add(as_IN);

            var az_Latn_AZ = new Language()
            {
                Name = "Azeri (Latin, Azerbaijan)",
                Code = "az-Latn-AZ",
                IsActive = true
            };
            languages.Add(az_Latn_AZ);

            var eu_ES = new Language()
            {
                Name = "Basque (Basque)",
                Code = "eu-ES",
                IsActive = true
            };
            languages.Add(eu_ES);

            var be_BY = new Language()
            {
                Name = "Belarusian (Belarus)",
                Code = "be_BY",
                IsActive = true
            };
            languages.Add(be_BY);

            var bn_BD = new Language()
            {
                Name = "Bengali (Bangladesh)",
                Code = "bn-BD",
                IsActive = true
            };
            languages.Add(bn_BD);

            var bn_IN = new Language()
            {
                Name = "Bengali (India)",
                Code = "bn-IN",
                IsActive = true
            };
            languages.Add(bn_IN);

            var bs_Cyrl_BA = new Language()
            {
                Name = "Bosnian (Cyrillic, Bosnia and Herzegovina)",
                Code = "bs-Cyrl-BA",
                IsActive = true
            };
            languages.Add(bs_Cyrl_BA);

            var bs_Latn_BA = new Language()
            {
                Name = "Bosnian (Latin, Bosnia and Herzegovina)",
                Code = "bs-Latn-BA",
                IsActive = true
            };
            languages.Add(bs_Latn_BA);

            var bg_BG = new Language()
            {
                Name = "Bulgarian (Bulgaria)",
                Code = "bg-BG",
                IsActive = true
            };
            languages.Add(bg_BG);

            var my_MM = new Language()
            {
                Name = "Burmese (Myanmar)",
                Code = "my-MM",
                IsActive = true
            };
            languages.Add(my_MM);

            var ca_ES = new Language()
            {
                Name = "Catalan (Catalan)",
                Code = "ca-ES",
                IsActive = true
            };
            languages.Add(ca_ES);

            var ku_Arab_IQ = new Language()
            {
                Name = "Central Kurdish (Iraq)",
                Code = "ku-Arab-IQ",
                IsActive = true
            };
            languages.Add(ku_Arab_IQ);

            var chr_Cher_US = new Language()
            {
                Name = "Cherokee (US)",
                Code = "chr-Cher-US",
                IsActive = true
            };
            languages.Add(chr_Cher_US);

            var zh_CN = new Language()
            {
                Name = "Chinese (Simplified, PRC)",
                Code = "zh-CN",
                IsActive = true
            };
            languages.Add(zh_CN);

            var zh_HK = new Language()
            {
                Name = "Chinese (Traditional, Hong Kong S.A.R.)",
                Code = "zh-HK",
                IsActive = true
            };
            languages.Add(zh_HK);

            var zh_TW = new Language()
            {
                Name = "Chinese (Traditional, Taiwan)",
                Code = "zh-TW",
                IsActive = true
            };
            languages.Add(zh_TW);

            var hr_HR = new Language()
            {
                Name = "Croatian (Croatia)",
                Code = "hr-HR",
                IsActive = true
            };
            languages.Add(hr_HR);

            var cs_CZ = new Language()
            {
                Name = "Czech (Czech Republic)",
                Code = "cs-CZ",
                IsActive = true
            };
            languages.Add(cs_CZ);

            var da_DK = new Language()
            {
                Name = "Danish (Denmark)",
                Code = "da-DK",
                IsActive = true
            };
            languages.Add(da_DK);

            var prs_AF = new Language()
            {
                Name = "Dari (Afghanistan)",
                Code = "prs-AF",
                IsActive = true
            };
            languages.Add(prs_AF);

            var nl_NL = new Language()
            {
                Name = "Dutch (Netherlands)",
                Code = "nl-NL",
                IsActive = true
            };
            languages.Add(nl_NL);

            var en_GB = new Language()
            {
                Name = "English (United Kingdom)",
                Code = "en-GB",
                IsActive = true
            };
            languages.Add(en_GB);

            var et_EE = new Language()
            {
                Name = "Estonian (Estonia)",
                Code = "et-EE",
                IsActive = true
            };
            languages.Add(et_EE);

            var fil_PH = new Language()
            {
                Name = "Filipino (Philippines)",
                Code = "fil-PH",
                IsActive = true
            };
            languages.Add(fil_PH);

            var fi_FI = new Language()
            {
                Name = "Finnish (Finland)",
                Code = "fi-FI",
                IsActive = true
            };
            languages.Add(fi_FI);

            var fr_CA = new Language()
            {
                Name = "French (Canada)",
                Code = "fr-CA",
                IsActive = true
            };
            languages.Add(fr_CA);

            var fr_FR = new Language()
            {
                Name = "French (France)",
                Code = "fr-FR",
                IsActive = true
            };
            languages.Add(fr_FR);

            var gl_ES = new Language()
            {
                Name = "Galician (Galician)",
                Code = "gl-ES",
                IsActive = true
            };
            languages.Add(gl_ES);

            var ka_GE = new Language()
            {
                Name = "Georgian (Georgia)",
                Code = "ka-GE",
                IsActive = true
            };
            languages.Add(ka_GE);

            var de_DE = new Language()
            {
                Name = "German (Germany)",
                Code = "de-DE",
                IsActive = true
            };
            languages.Add(de_DE);


            var el_GR = new Language()
            {
                Name = "Greek (Greece)",
                Code = "el-GR",
                IsActive = true
            };
            languages.Add(el_GR);

            var gu_IN = new Language()
            {
                Name = "Gujarati (India)",
                Code = "gu-IN",
                IsActive = true
            };
            languages.Add(gu_IN);

            var ha_Latn_NG = new Language()
            {
                Name = "Hausa (Latin, Nigeria)",
                Code = "ha-Latn-NG",
                IsActive = true
            };
            languages.Add(ha_Latn_NG);

            var he_IL = new Language()
            {
                Name = "Hebrew (Israel)",
                Code = "he-IL",
                IsActive = true
            };
            languages.Add(he_IL);

            var hi_IN = new Language()
            {
                Name = "Hindi (India)",
                Code = "hi-IN",
                IsActive = true
            };
            languages.Add(hi_IN);

            var hu_HU = new Language()
            {
                Name = "Hungarian (Hungary)",
                Code = "hu-HU",
                IsActive = true
            };
            languages.Add(hu_HU);

            var is_IS = new Language()
            {
                Name = "Icelandic (Iceland)",
                Code = "is-IS",
                IsActive = true
            };
            languages.Add(is_IS);

            var ig_NG = new Language()
            {
                Name = "Igbo (Nigeria)",
                Code = "ig-NG",
                IsActive = true
            };
            languages.Add(ig_NG);

            var id_ID = new Language()
            {
                Name = "Indonesian (Indonesia)",
                Code = "id-LanguageID",
                IsActive = true
            };
            languages.Add(id_ID);

            var ga_IE = new Language()
            {
                Name = "Irish (Ireland)",
                Code = "ga-IE",
                IsActive = true
            };
            languages.Add(ga_IE);

            var xh_ZA = new Language()
            {
                Name = "isiXhosa (South Africa)",
                Code = "xh-ZA",
                IsActive = true
            };
            languages.Add(xh_ZA);

            var zu_ZA = new Language()
            {
                Name = "isiZulu (South Africa)",
                Code = "zu-ZA",
                IsActive = true
            };
            languages.Add(zu_ZA);

            var it_IT = new Language()
            {
                Name = "Italian (Italy)",
                Code = "it-IT",
                IsActive = true
            };
            languages.Add(it_IT);

            var ja_JP = new Language()
            {
                Name = "Japanese (Japan)",
                Code = "ja-JP",
                IsActive = true
            };
            languages.Add(ja_JP);

            var kn_IN = new Language()
            {
                Name = "Kannada (India)",
                Code = "kn-IN",
                IsActive = true
            };
            languages.Add(kn_IN);

            var kk_KZ = new Language()
            {
                Name = "Kazakh (Kazakhstan)",
                Code = "kk-KZ",
                IsActive = true
            };
            languages.Add(kk_KZ);

            var km_KH = new Language()
            {
                Name = "Khmer (Cambodia)",
                Code = "km-KH",
                IsActive = true
            };
            languages.Add(km_KH);

            var quc_Latn_GT = new Language()
            {
                Name = "K'iche (Guatemala)",
                Code = "quc-Latn-GT",
                IsActive = true
            };
            languages.Add(quc_Latn_GT);

            var rw_RW = new Language()
            {
                Name = "Kinyarwanda (Rwanda)",
                Code = "rw-RW",
                IsActive = true
            };
            languages.Add(rw_RW);

            var sw_KE = new Language()
            {
                Name = "Kiswahili (Kenya)",
                Code = "sw-KE",
                IsActive = true
            };
            languages.Add(sw_KE);

            var kok_IN = new Language()
            {
                Name = "Konkani (India)",
                Code = "kok-IN",
                IsActive = true
            };
            languages.Add(kok_IN);

            var ko_KR = new Language()
            {
                Name = "Korean (Korea)",
                Code = "ko-KR",
                IsActive = true
            };
            languages.Add(ko_KR);

            var ky_KG = new Language()
            {
                Name = "Kyrgyz (Kyrgyzstan)",
                Code = "ky-KG",
                IsActive = true
            };
            languages.Add(ky_KG);

            var lo_LA = new Language()
            {
                Name = "Lao (Lao P.D.R.)",
                Code = "lo-LA",
                IsActive = true
            };
            languages.Add(lo_LA);

            var lv_LV = new Language()
            {
                Name = "Latvian (Latvia)",
                Code = "lv-LV",
                IsActive = true
            };
            languages.Add(lv_LV);

            var lt_LT = new Language()
            {
                Name = "Lithuanian (Lithuania)",
                Code = "lt-LT",
                IsActive = true
            };
            languages.Add(lt_LT);

            var lb_LU = new Language()
            {
                Name = "Luxembourgish (Luxembourg)",
                Code = "lb-LU",
                IsActive = true
            };
            languages.Add(lb_LU);

            var mk_MK = new Language()
            {
                Name = "Macedonian (FYRO Macedonia)",
                Code = "mk-MK",
                IsActive = true
            };
            languages.Add(mk_MK);

            var ms_MY = new Language()
            {
                Name = "Malay (Malaysia)",
                Code = "ms-MY",
                IsActive = true
            };
            languages.Add(ms_MY);

            var ml_IN = new Language()
            {
                Name = "Malayalam (India)",
                Code = "ml-IN",
                IsActive = true
            };
            languages.Add(ml_IN);

            var mt_MT = new Language()
            {
                Name = "Maltese (Malta)",
                Code = "mt-MT",
                IsActive = true
            };
            languages.Add(mt_MT);

            var mi_NZ = new Language()
            {
                Name = "Maori (New Zealand)",
                Code = "mi-NZ",
                IsActive = true
            };
            languages.Add(mi_NZ);

            var mr_IN = new Language()
            {
                Name = "Marathi (India)",
                Code = "mr-IN",
                IsActive = true
            };
            languages.Add(mr_IN);

            var mn_MN = new Language()
            {
                Name = "Mongolian (Cyrillic, Mongolia)",
                Code = "mn-MN",
                IsActive = true
            };
            languages.Add(mn_MN);

            var ne_NP = new Language()
            {
                Name = "Nepali (Nepal)",
                Code = "ne-NP",
                IsActive = true
            };
            languages.Add(ne_NP);

            var nb_NO = new Language()
            {
                Name = "Norwegian, Bokmål (Norway)",
                Code = "nb-NO",
                IsActive = true
            };
            languages.Add(nb_NO);

            var nn_NO = new Language()
            {
                Name = "Norwegian, Nynorsk (Norway)",
                Code = "nn-NO",
                IsActive = true
            };
            languages.Add(nn_NO);

            var or_IN = new Language()
            {
                Name = "Oriya (India)",
                Code = "or-IN",
                IsActive = true
            };
            languages.Add(or_IN);

            var fa_IR = new Language()
            {
                Name = "Persian",
                Code = "fa-IR",
                IsActive = true
            };
            languages.Add(fa_IR);

            var pl_PL = new Language()
            {
                Name = "Polish (Poland)",
                Code = "pl-PL",
                IsActive = true
            };
            languages.Add(pl_PL);

            var pt_BR = new Language()
            {
                Name = "Portuguese (Brazil)",
                Code = "pt-BR",
                IsActive = true
            };
            languages.Add(pt_BR);

            var pt_PT = new Language()
            {
                Name = "Portuguese (Portugal)",
                Code = "pt-PT",
                IsActive = true
            };
            languages.Add(pt_PT);

            var pa_IN = new Language()
            {
                Name = "Punjabi (India)",
                Code = "pa-IN",
                IsActive = true
            };
            languages.Add(pa_IN);

            var pa_Arab_PK = new Language()
            {
                Name = "Punjabi (Pakistan)",
                Code = "pa-Arab-PK",
                IsActive = true
            };
            languages.Add(pa_Arab_PK);

            var quz_PE = new Language()
            {
                Name = "Quechua (Peru)",
                Code = "quz-PE",
                IsActive = true
            };
            languages.Add(quz_PE);

            var ro_RO = new Language()
            {
                Name = "Romanian (Romania)",
                Code = "ro-RO",
                IsActive = true
            };
            languages.Add(ro_RO);

            var ru_RU = new Language()
            {
                Name = "Russian (Russia)",
                Code = "ru-RU",
                IsActive = true
            };
            languages.Add(ru_RU);

            var gd_GB = new Language()
            {
                Name = "Scottish Gaelic (United Kingdom)",
                Code = "gd-GB",
                IsActive = true
            };
            languages.Add(gd_GB);

            var sr_Cyrl_BA = new Language()
            {
                Name = "Serbian (Cyrillic, Bosnia and Herzegovina)",
                Code = "sr-Cyrl-BA",
                IsActive = true
            };
            languages.Add(sr_Cyrl_BA);

            var sr_Cyrl_RS = new Language()
            {
                Name = "Serbian (Cyrillic, Serbia)",
                Code = "sr-Cyrl-RS",
                IsActive = true
            };
            languages.Add(sr_Cyrl_RS);

            var sr_Latn_RS = new Language()
            {
                Name = "Serbian (Latin, Serbia)",
                Code = "sr-Latn-RS",
                IsActive = true
            };
            languages.Add(sr_Latn_RS);

            var nso_ZA = new Language()
            {
                Name = "Sesotho sa Leboa (South Africa)",
                Code = "nso-ZA",
                IsActive = true
            };
            languages.Add(nso_ZA);

            var tn_ZA = new Language()
            {
                Name = "Setswana (South Africa)",
                Code = "tn-ZA",
                IsActive = true
            };
            languages.Add(tn_ZA);

            var sd_Arab_PK = new Language()
            {
                Name = "Sindhi (Pakistan)",
                Code = "sd-Arab-PK",
                IsActive = true
            };
            languages.Add(sd_Arab_PK);

            var si_LK = new Language()
            {
                Name = "Sinhala (Sri Lanka)",
                Code = "si-LK",
                IsActive = true
            };
            languages.Add(si_LK);

            var sk_SK = new Language()
            {
                Name = "Slovak (Slovakia)",
                Code = "sk-SK",
                IsActive = true
            };
            languages.Add(sk_SK);

            var sl_SI = new Language()
            {
                Name = "Slovenian (Slovenia)",
                Code = "sl-SI",
                IsActive = true
            };
            languages.Add(sl_SI);

            var es_MX = new Language()
            {
                Name = "Spanish (Mexico)",
                Code = "es-MX",
                IsActive = true
            };
            languages.Add(es_MX);

            var es_ES = new Language()
            {
                Name = "Spanish (Spain)",
                Code = "es-ES",
                IsActive = true
            };
            languages.Add(es_ES);

            var sv_SE = new Language()
            {
                Name = "Swedish (Sweden)",
                Code = "sv-SE",
                IsActive = true
            };
            languages.Add(sv_SE);

            var tg_Cyrl_TJ = new Language()
            {
                Name = "Tajik (Cyrillic, Tajikistan)",
                Code = "tg-Cyrl-TJ",
                IsActive = true
            };
            languages.Add(tg_Cyrl_TJ);

            var ta_IN = new Language()
            {
                Name = "Tamil (India)",
                Code = "ta-IN",
                IsActive = true
            };
            languages.Add(ta_IN);

            var tt_RU = new Language()
            {
                Name = "Tatar (Russia)",
                Code = "tt-RU",
                IsActive = true
            };
            languages.Add(tt_RU);

            var te_IN = new Language()
            {
                Name = "Telugu (India)",
                Code = "te-IN",
                IsActive = true
            };
            languages.Add(te_IN);

            var th_TH = new Language()
            {
                Name = "Thai (Thailand)",
                Code = "th-TH",
                IsActive = true
            };
            languages.Add(th_TH);

            var tr_TR = new Language()
            {
                Name = "Turkish (Turkey)",
                Code = "tr-TR",
                IsActive = true
            };
            languages.Add(tr_TR);

            var tk_TM = new Language()
            {
                Name = "Turkmen (Turkmenistan)",
                Code = "tk-TM",
                IsActive = true
            };
            languages.Add(tk_TM);

            var uk_UA = new Language()
            {
                Name = "Ukrainian (Ukraine)",
                Code = "uk-UA",
                IsActive = true
            };
            languages.Add(uk_UA);

            var ur_PK = new Language()
            {
                Name = "Urdu (Islamic Republic of Pakistan)",
                Code = "ur-PK",
                IsActive = true
            };
            languages.Add(ur_PK);

            var ug_CN = new Language()
            {
                Name = "Uyghur (PRC)",
                Code = "ug-CN",
                IsActive = true
            };
            languages.Add(ug_CN);

            var uz_Latn_UZ = new Language()
            {
                Name = "Uzbek (Latin, Uzbekistan)",
                Code = "uz-Latn-UZ",
                IsActive = true
            };
            languages.Add(uz_Latn_UZ);

            var ca_ES_valencia = new Language()
            {
                Name = "Valencian (Spain)",
                Code = "ca-ES-valencia",
                IsActive = true
            };
            languages.Add(ca_ES_valencia);

            var vi_VN = new Language()
            {
                Name = "Vietnamese (Vietnam)",
                Code = "vi-VN",
                IsActive = true
            };
            languages.Add(vi_VN);

            var cy_GB = new Language()
            {
                Name = "Welsh (United Kingdom)",
                Code = "cy-GB",
                IsActive = true
            };
            languages.Add(cy_GB);

            var wo_SN = new Language()
            {
                Name = "Wolof (Senegal)",
                Code = "wo-SN",
                IsActive = true
            };
            languages.Add(wo_SN);

            var yo_NG = new Language()
            {
                Name = "Yoruba (Nigeria)",
                Code = "yo-NG",
                IsActive = true
            };
            languages.Add(yo_NG);

            var ti_ET = new Language()
            {
                Name = "Tigrinya (Ethiopia)",
                Code = "ti-ET",
                IsActive = true
            };
            languages.Add(ti_ET);

            var en_US = new Language()
            {
                Name = "English (United States)",
                Code = "en-US",
                IsActive = true
            };
            languages.Add(en_US);

            return languages;
        }
    }
}
