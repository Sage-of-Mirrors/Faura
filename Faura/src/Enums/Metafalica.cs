﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura.Enums.Metafalica
{
    public enum SoundID : int
    {
        dummy = 0
    }

    public enum MusicID : int
    {
        dummy = 0
    }

    public enum EnemyID : int
    {
        dummy = 0
    }

    public enum CharacterNameID : int
    {
        NONE = -1,
        LUCA = 0,
        CLOCHE = 1,
        JACQLI = 2,
        CROIX = 3,
        LEGLIUS = 4,
        AMARIE = 5,
        SHUN = 6,
        COCONA = 7,
        TARGANA = 10,
        LAUDE = 13,
        CHESTER = 15,
        BATZ = 16,
        REISHA = 17,
        FRELIA = 20,
        INFEL = 25,
        CYNTHIA = 27,
        SOOPE = 31,
        DIVE_SHOP_LENLEN = 99,
        AYATANE = 135
    }

    public enum PortraitSlot : int
    {
        CENTER = 0,
        LEFT1 = 1,
        LEFT2 = 2,
        RIGHT1 = 3,
        RIGHT2 = 4
    }

    public enum PortraitPosition : int
    {
        NONE = -1,
        CENTERED = 0,
        LEFT_CONVERSATION = 1,
        LEFT_ALONE = 2,
        RIGHT_CONVERSATION = 3,
        RIGHT_ALONE = 4
    }

    public enum TextBoxType : int
    {
        NORMAL_DIALOG = 0,
        ROUNDED_BUBBLY = 1,
        FULL_PORTRAIT = 2,
        UNKNOWN1 = 3,
        TOP_CENTERED = 4,
        BOTTOM_CENTERED = 5,
        STRETCHED_ACROSS_BOTTOM = 6,
        INVALID1 = 7,
        BOTTOM_RIGHT = 8,
        PORTRAIT_ACROSS_BOTTOM = 9,
        BOTTOM_CENTERED_2 = 10,
        ROUNDED_BUBBLY_BOTTOM_CENTER = 11,
        COSMOSPHERE_CENTER_PORTRAIT = 12
    }

    public enum TextboxMode : int
    {
        SNAP_TO_SPEAKER,
        SINGLE_TEXTBOX
    }

    public enum VisibilityEnum : int
    {
        INVISIBLE,
        VISIBLE
    }

    public enum GraphicsMode : int
    {
        COSMOSPHERE,
        OVERWORLD
    }

    public enum CharacterID : int
    {
        LUCA = 0,
        CLOCHE = 1,
        JACQLI = 2,
        CROIX = 3,
        LEGLIUS = 4,
        AMARIE = 5,
        SHUN = 6,
        COCONA = 7,
        LUCA_2 = 8,
        LUCA_3 = 9,
        CROIX_2 = 10,
        CROIX_3 = 11,
        TARGANA = 12,
        LAKRA = 13,
        LAUDE = 14,
        CHESTER = 15,
        ALFMAN = 16,
        BATZ = 17,
        REISHA = 18,
        RAKI = 19,
        REKI = 20,
        FRELIA = 21,
        LIL_LUCA = 22,
        LIL_CLOCHE = 23,
        JACQLI_ROBOT = 24,
        JACQLI_ROBOT_V2 = 25,
        INFEL = 26,
        NENESHA = 27,
        CYNTHIA = 28,
        SPICA = 29,
        SASHA = 30,
        SKYCAT = 31,
        SOOPE = 32,
        ARSHE = 33,
        LIL_CROIX = 34,
        LIL_TARGANA = 35,
        GRAND_BELL_KNIGHT_A = 36,
        GRAND_BELL_KNIGHT_B = 37,
        GRAND_BELL_KNIGHT_C = 38,
        GRAND_BELL_KNIGHT_D = 39,
        GRAND_BELL_KNIGHT_E = 40,
        GRAND_BELL_KNIGHT_F = 41,
        GRAND_BELL_KNIGHT_G = 42,
        GRAND_BELL_KNIGHT_H = 43,
        GRAND_BELL_KNIGHT_I = 44,
        GRAND_BELL_KNIGHT_J = 45,
        SACRED_ARMY_A = 46,
        SACRED_ARMY_B = 47,
        SACRED_ARMY_C = 48,
        SACRED_ARMY_D = 49,
        SACRED_ARMY_E = 50,
        SACRED_ARMY_F = 51,
        SACRED_ARMY_G = 52,
        YOUNG_MAN_A = 53,
        YOUNG_MAN_B = 54,
        YOUNG_MAN_C = 55,
        RAKSHEK_REP = 56,
        DAUGHTER_A = 57,
        DAUGHTER_B = 58,
        DAUGHTER_C = 59,
        DAUGHTER_D = 60,
        DAUGHTER_E = 61,
        DAUGHTER_F = 62,
        DAUGHTER_G = 63,
        DAUGHTER_H = 64,
        BOY_A = 65,
        BOY_B = 66,
        BOY_C = 67,
        JEAN = 68,
        GIRL_A = 69,
        GIRL_B = 70,
        NYOCHAN = 71,
        GIRL_LIL_SIS = 72,
        MAID = 73,
        UNCLE_A = 74,
        UNCLE_B = 75,
        ENNACOMMUNITYLEADER = 76,
        AUNTIE_A = 77,
        AUNTIE_B = 78,
        ENNA_GOVERNOR = 79,
        MRS_LAHR = 80,
        NOBLE_LADY_A = 81,
        NOBLE_LADY_B = 82,
        NOBLE_LADY_SEXY = 83,
        NANA = 84,
        GRANDPA_A = 85,
        MINT_BLOCK_MAYOR = 86,
        GRANNY_B = 87,
        ADVENTURER_A = 88,
        ADVENTURER_B = 89,
        ADVENTURER_C = 90,
        ADVENTURER_D = 91,
        TERU_MAN_A = 92,
        TERU_WOMAN_A = 93,
        ELMA_A = 94,
        ELMA_DSRX = 95,
        PIPPEN_A = 96,
        PIPPEN_B = 97,
        PIPPEN_C = 98,
        PIPPEN_D = 99,
        PIPPEN_E = 100,
        PIPPEN_F = 101,
        PIPPEN_G = 102,
        PIPPEN_H = 103,
        PIPPEN_I = 104,
        PIPPEN_J = 105,
        PIPPEN_K = 106,
        PIPPEN_L = 107,
        PIPPEN_M = 108,
        PIPPEN_N = 109,
        PIPPEN_O = 110,
        PIPPEN_P = 111,
        PIPPEN_Q = 112,
        PIPPEN_R = 113,
        PIPPEN_S = 114,
        PIPPEN_T = 115,
        PIPPEN_U = 116,
        PIPPEN_V = 117,
        PIPPEN_W = 118,
        PIPPEN_X = 119,
        PIPPEN_Y = 120,
        PIPPEN_Z = 121,
        PIPPIPPEN = 122,
        SOOPE_A = 123,
        SOOPE_B = 124,
        SOOPE_C = 125,
        SOOPE_D = 126,
        SOOPE_E = 127,
        SOOPE_F = 128,
        SOOPE_G = 129,
        SOOPE_H = 130,
        SOOPE_I = 131,
        SOOPE_J = 132,
        SOOPE_K = 133,
        SOOPE_L = 134,
        SOOPLI = 135,
        SOOPLU = 136,
        SOOPLE = 137,
        STEEL_ANGEL_A = 138,
        STEEL_ANGEL_B = 139,
        STEEL_ANGEL_C = 140,
        STEEL_ANGEL_D = 141,
        STEEL_ANGEL_E = 142,
        STEEL_ANGEL_F = 143,
        STEEL_ANGEL_G = 144,
        STEEL_ANGEL_H = 145,
        STEEL_ANGEL_I = 146,
        STEEL_ANGEL_J = 147,
        STEEL_ANGEL_K = 148,
        STEEL_ANGEL_L = 149,
        DEVIL_A = 150,
        DEVIL_B = 151,
        DEVIL_C = 152,
        DEVIL_D = 153,
        DEVIL_E = 154,
        DEVIL_F = 155,
        DEVIL_G = 156,
        DEVIL_H = 157,
        DEVIL_I = 158,
        DEVIL_J = 159,
        DEVIL_K = 160,
        DEVIL_L = 161,
        REYVATEIL_A = 162,
        REYVATEIL_B = 163,
        REYVATEIL_C = 164,
        REYVATEIL_D = 165,
        REYVATEIL_E = 166,
        REYVATEIL_F = 167,
        REYVATEIL_G = 168,
        REYVATEILBIGSIS = 169,
        REYVATEILLILSIS = 170,
        NATTIE = 171,
        JENICA = 172,
        LENNY = 173,
        CLOCHES_BODYGUARD = 174,
        CLOCHES_BODYGUARD_1 = 175,
        CLOCHES_BODYGUARD_2 = 176,
        CLOCHES_BODYGUARD_3 = 177,
        CLOCHES_BODYGUARD_4 = 178,
        DIVE_SHOP_LELEN = 179,
        FANCY_SHOP_WORKER = 180,
        FANCY_SHOP_WORKER_2 = 181,
        DIVE_SHOP_GEN = 182,
        RAKSHEK_WORKER = 183,
        RAKSHEK_INN = 184,
        ENNA_WORKER = 185,
        ENNA_INN = 186,
        MINT_BLOCK_WORKER = 187,
        RESORT_WORKER = 188,
        GAEA_WORKER = 189,
        GAEA_INNKEEPER = 190,
        GAEA_DIVE_SHOP = 191,
        DUMMY_A = 192,
        DUMMY_B = 193,
        DUMMY_C = 194,
        DUMMY_D = 195,
        DUMMY_E = 196,
        DUMMY_F = 197,
        DUMMY_G = 198,
        DUMMY_H = 199,
        DUMMY_I = 200,
        DUMMY_J = 201,
        LUCA_COPY = 202,
        LUCA_COPY_A = 203,
        LUCA_COPY_B = 204,
        LUCA_COPY_C = 205,
        LUCA_COPY_D = 206,
        LUCA_COPY_E = 207,
        CLOCHE_COPY = 208,
        CLOCHE_COPY_A = 209,
        CLOCHE_COPY_B = 210,
        CLOCHE_COPY_C = 211,
        CLOCHE_COPY_D = 212,
        CLOCHE_COPY_E = 213,
        AUNTIE = 214,
        UNCLE = 215,
        UNCLE_A_2 = 216,
        UNCLE_B_2 = 217,
        OLD_LADY = 218,
        OLD_MAN = 219,
        DAUGHTER = 220,
        GIRL = 221,
        GIRL_A_2 = 222,
        GIRL_B_2 = 223,
        BIG_SIS = 224,
        BOY = 225,
        BOY_A_2 = 226,
        BOY_B_2 = 227,
        BIG_BRO = 228,
        YOUNG_MAN = 229,
        TRAIN_CONDUCTOR = 230,
        MAID_2 = 231,
        KNIGHT = 232,
        KNIGHT_A = 233,
        KNIGHT_B = 234,
        KNIGHT_C = 235,
        KNIGHT_D = 236,
        KNIGHT_SQUAD = 237,
        SACRED_KNIGHT = 238,
        SACRED_KNIGHT_A = 239,
        SACRED_KNIGHT_B = 240,
        SACRED_KNIGHT_C = 241,
        REYVATEIL = 242,
        REYVATEIL_A_2 = 243,
        REYVATEIL_B_2 = 244,
        REYVATEIL_C_2 = 245,
        REYVATEIL_SACRED = 246,
        REYVATEIL_SACRED_A = 247,
        REYVATEIL_SACRED_B = 248,
        REYVATEIL_SACRED_C = 249,
        REYVATEIL_SACRED_D = 250,
        REYVATEIL_SACRED_E = 251,
        REYVATEIL_SACRED_F = 252,
        REYVATEIL_SACRED_G = 253,
        SQUAD_LEADER = 254,
        DIVINE_ARMY_A = 255,
        DIVINE_ARMY_B = 256,
        ELMA = 257,
        NECROMANCER = 258,
        NECROMANCER_A = 259,
        NECROMANCER_B = 260,
        STEEL_ANGEL = 261,
        STEEL_ANGEL_A_2 = 262,
        STEEL_ANGEL_B_2 = 263,
        SPECIAL_ANM_CLUSTER = 264,
        CRYSTAL = 265,
        JOURNAL = 266,
        DUMMY_Z = 267,
        CYNTHIA_COPY = 268,
        SPICA_COPY = 269,
        SASHA_COPY = 270,
        SKYCAT_COPY = 271,
        DIVE_SHOP_GEN_COPY = 272,
        LUCA_BARTENDER = 273,
        LUCA_RITUAL = 274,
        LUCA_PRETTYCURE = 275,
        LUCA_ASSAULT = 276,
        LUCA_WITCH = 277,
        LUCA_ULTIMATE_WEAPON = 278,
        LUCA_BATH_TOWEL = 279,
        LUCA_WHITE_KIMONO = 280,
        LUCA_JAPANESE_GAL = 281,
        CLOCHE_JAPANESE_KILT = 282,
        CLOCHE_UNIFORM = 283,
        CLOCHE_KIMONO = 284,
        CLOCHE_VICTIM = 285,
        CLOCHE_PRETTYCURE = 286,
        CLOCHE_DEVIL = 287,
        CLOCHE_PAJAMA = 288,
        CLOCHE_WEDDING = 289,
        CLOCHE_PIPPEN = 290,
        JACQLI_COPY = 291,
        JACQLI_GUYS_UNIFORM = 292,
        JACQLI_MAID = 293,
        JACQLI_PE_CLOTHES = 294,
        JACQLI_SWIMSUIT = 295,
        JACQLI_FUNBUN = 296,
        JACQLI_SLEEPWEAR = 297,
        JACQLI_SHADOW = 298,
        JACQLI_DRESS = 299,
        JACQLI_WEDDING = 300
    }

    public enum PortraitID : int
    {
        NONE = -1,

        LUCA_SMILE = 0,
        LUCA_LAUGH = 1,
        LUCA_NEUTRAL = 2,
        LUCA_LIP_BITE = 3,
        LUCA_GASP_WITH_TEAR = 4,
        LUCA_TONGUE_OUT = 5,
        LUCA_BORED = 6,
        LUCA_CRYING = 7,
        LUCA_YELLING = 8,
        LUCA_BLUSHING = 9,
        LUCA_EMOTIONLESS = 10,
        LUCA_SINGLE_TEAR = 11,
        LUCA_SEDUCTIVE = 12,
        LUCA_SMILE_2 = 13,
        LUCA_GASP = 14,

        LEGLIUS_NEUTRAL = 469,
        LEGLIUS_SNARL = 470,
        LEGLIUS_SMILE = 471,
        LEGLIUS_SHOCKED = 472,
        LEGLIUS_DEJECTED = 473,
        LEGLIUS_CONCERNED = 474,

        SHUN_NEUTRAL = 475,
        SHUN_SMIRK = 476,
        SHUN_ROAR = 477,
        SHUN_GROWL = 478,
        SHUN_SMILE = 479,

        FRELIA_SMILE = 538,
        FRELIA_GRIN = 539,
        FRELIA_CRYING = 540,
        FRELIA_DEJECTED = 541,
        FRELIA_BLUSH = 542,
        FRELIA_EXPRESSIONLESS = 543,

        INFEL_SMILE = 589, // A
        INFEL_SMIRK = 590, // B
        INFEL_FROWN = 591, // C
        INFEL_EMBARRASSED = 592, // D
        INFEL_SNARL = 593, // E
        INFEL_GRIN = 594, // F
        INFEL_UNAMUSED = 595, // G
        INFEL_NERVOUS = 596, // H
        INFEL_WAILING = 597, // I
    }
}
