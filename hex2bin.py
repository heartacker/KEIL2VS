#!/usr/bin/env python3
#coding=utf-8
"""
Function: Convert the files in the hex format into the .bin files of different projects.
Copyright information: Huawei Technologies Co., Ltd. All versions (C) 2018-2019
Modification record: 2018-12-14 12:00 lwx460481 Creation
Version record: V1.0.0.0

Modification record: 2018-12-14 16:56:58 lwx460481
1. H15 and H16 are supported.
Version record: V1.0.0.2

Modification record: 2018-12-14 17:37:33 lwx460481
1. Rectify the enumeration bug on the server.
Version record: V1.0.0.3

Modification record: 2018-12-17 11:48:54 lwx460481
1. Change the name H28V101_DFE to H28V101_ADAPT.
2. Modify the configuration parameters of the H28V200.
Version record: V1.0.0.4

Modification record: 2018-12-21 09:49:37 lwx460481
1. Add the version identification and generate the bin file with the version.
2. Generating a bin File with a Date
3. Add the version identification and generate the .txt file with the version and date.
4. Other minor problems are modified.
Version record: V1.2.0.0

Modification record: 2018-12-21 18:22:14 lwx460481
1. The configuration parameters of the H12 (H12_DFE_1_12)(H12_CTLE)(H12_DFE) of the earlier version are added.
Version record: V1.2.0.1

Modification record: 2019-1-3 15:03:56 lwx460481
1. Resolve the problem that the h30 cannot identify the version number.
2. The command help information is added.
3. CRC support, supporting 8 16 32
4. Other General Amendments
Version record: V2.0.0.0
"""

import os
import time
import re
import argparse
#from enum import Enum, unique
"""
Hex code Record type
    0     Data
    1     End Of File
    2     Extended Segment Address
    3     Start Segment Address
    4     Extended Linear Address
    5     Start Linear Address
"""

#支持的项目及参数
gl_proj_list = ["H32T7V101",
             "H32T7V100",
             "H12",
             "H12_DFE_1_12",
             "H12_CTLE",
             "H12_DFE",
             "H15",
             "H16",
             "H28V101_CALIB",
             "H30",
             "H28V101_ADAPT",
             "H28V200",
             "PonLink10T28",
    "H60LRV100"]
gl_CS_BaseAddress = 0xFFE6
gl_CODE_LENGTH_STAT_API = gl_CS_BaseAddress + 0x14
gl_CRC_Address = 0xFEC2
def get_crc_table(crc_type):
    """ 获取crc tabel"""
    if crc_type == 32:
        CRC32_Table = [0x00000000, 0x77073096, 0xee0e612c, 0x990951ba,
                       0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3,
                       0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988,
                       0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
                       0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de,
                       0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
                       0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec,
                       0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
                       0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172,
                       0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
                       0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940,
                       0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
                       0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116,
                       0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f,
                       0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
                       0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
                       0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a,
                       0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
                       0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818,
                       0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
                       0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e,
                       0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457,
                       0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c,
                       0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
                       0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2,
                       0x4adfa541, 0x3dd895d7,
    elif crc_type == 8:
        CRC8_Table = [0x00, 0x5E, 0xBC, 0xE2, 0x61, 0x3F, 0xDD, 0x83,
                      0xC2, 0x9C, 0x7E, 0x20, 0xA3, 0xFD, 0x1F, 0x41,
                      0x9D, 0xC3, 0x21, 0x7F, 0xFC, 0xA2, 0x40, 0x1E,
                      0x5F, 0x01, 0xE3, 0xBD, 0x3E, 0x60, 0x82, 0xDC,
                      0x23, 0x7D, 0x9F, 0xC1, 0x42, 0x1C, 0xFE, 0xA0,
                      0xE1, 0xBF, 0x5D, 0x03, 0x80, 0xDE, 0x3C, 0x62,
                      0xBE, 0xE0, 0x02, 0x5C, 0xDF, 0x81, 0x63, 0x3D,
                      0x7C, 0x22, 0xC0, 0x9E, 0x1D, 0x43, 0xA1, 0xFF,
                      0x46, 0x18, 0xFA, 0xA4, 0x27, 0x79, 0x9B, 0xC5,
                      0x84, 0xDA, 0x38, 0x66, 0xE5, 0xBB, 0x59, 0x07,
                      0xDB, 0x85, 0x67, 0x39, 0xBA, 0xE4, 0x06, 0x58,
                      0x19, 0x47, 0xA5, 0xFB, 0x78, 0x26, 0xC4, 0x9A,
                      0x65, 0x3B, 0xD9, 0x87, 0x04, 0x5A, 0xB8, 0xE6,
                      0xA7, 0xF9, 0x1B, 0x45, 0xC6, 0x98, 0x7A, 0x24,
                      0xF8, 0xA6, 0x44, 0x1A, 0x99, 0xC7, 0x25, 0x7B,
                      0x3A, 0x64, 0x86, 0xD8, 0x5B, 0x05, 0xE7, 0xB9,
                      0x8C, 0xD2, 0x30, 0x6E, 0xED, 0xB3, 0x51, 0x0F,
                      0x4E, 0x10, 0xF2, 0xAC, 0x2F, 0x71, 0x93, 0xCD,
                      0x11, 0x4F, 0xAD, 0xF3, 0x70, 0x2E, 0xCC, 0x92,
                      0xD3, 0x8D, 0x6F, 0x31, 0xB2, 0xEC, 0x0E, 0x50,
                      0xAF, 0xF1, 0x13, 0x4D, 0xCE, 0x90, 0x72, 0x2C,
                      0x6D, 0x33, 0xD1, 0x8F, 0x0C, 0x52, 0xB0, 0xEE,
                      0x32, 0x6C, 0x8E, 0xD0, 0x53, 0x0D, 0xEF, 0xB1,
                      0xF0, 0xAE, 0x4C, 0x12, 0x91, 0xCF, 0x2D, 0x73,
                      0xCA, 0x94, 0x76, 0x28, 0xAB, 0xF5, 0x17, 0x49,
                      0x08, 0x56, 0xB4, 0xEA, 0x69, 0x37, 0xD5, 0x8B,
                      0x57, 0x09, 0xEB, 0xB5, 0x36, 0x68, 0x8A, 0xD4,
                      0x95, 0xCB, 0x29, 0x77, 0xF4, 0xAA, 0x48, 0x16,
                      0xE9, 0xB7, 0x55, 0x0B, 0x88, 0xD6, 0x34, 0x6A,
                      0x2B, 0x75, 0x97, 0xC9, 0x4A, 0x14, 0xF6, 0xA8,
                      0x74, 0x2A, 0xC8, 0x96, 0x15, 0x4B, 0xA9, 0xF7,
                      0xB6, 0xE8, 0x0A, 0x54, 0xD7, 0x89, 0x6B, 0x35]
        return CRC8_Table
    else:
        raise("crc type input error,please input 8,16 or 32")

ef get_config(project_name):
    """
    功能描述：获取项目的配置
    参数：项目名称
    返回值：项目配置的字典
    异常描述：无法匹配的返回None
    修改记录
    """
    """
    支持项目的配置
    1. H32T7V101
    2. H32T7V100
    3. H12 (H12_DFE_1_12)(H12_CTLE)(H12_DFE)
    4. H15
    5. H16
    6. H28V101_CALIB
    7. H30
    8. H28V101_ADAPT
    9. H28V200
    10.PonLink10T28
 11.H60LRV100
    """
    config = {}
    if project_name == "H12_CTLE":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3400
        config['file_len'] = 0x3400
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12_DFE":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3C00
        config['file_len'] = 0x3C00
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12_DFE_1_12":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3FF0
        config['file_len'] = 0x3FF0
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3FF0
        config['file_len'] = 0x3FF0
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H32T7V101':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x10000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H32T7V100':
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0X30000
        config['file_len'] = 0x30000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif (project_name == 'H15' or project_name == 'H16' or project_name == 'H28V101_CALIB' or project_name == 'H30'):
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0x8000
        config['file_len'] = 0x8000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H28V101_ADAPT':
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0x7E50
        config['file_len'] = 0x7E50
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H28V200':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0xF000
        config['file_len'] = 0x8000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'PonLink10T28':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x4000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H60LRV100':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x10000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    else:
        return None
    return config


def get_config(project_name):
    """
    功能描述：获取项目的配置
    参数：项目名称
    返回值：项目配置的字典
    异常描述：无法匹配的返回None
    修改记录
    """
    """
    支持项目的配置
    1. H32T7V101
    2. H32T7V100
    3. H12 (H12_DFE_1_12)(H12_CTLE)(H12_DFE)
    4. H15
    5. H16
    6. H28V101_CALIB
    7. H30
    8. H28V101_ADAPT
    9. H28V200
    10.PonLink10T28
 11.H60LRV100
    """
    config = {}
    if project_name == "H12_CTLE":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3400
        config['file_len'] = 0x3400
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12_DFE":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3C00
        config['file_len'] = 0x3C00
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12_DFE_1_12":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3FF0
        config['file_len'] = 0x3FF0
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == "H12":
        config['clip_addr_base'] = 0x4000
        config['bin_max_len'] = 0x3FF0
        config['file_len'] = 0x3FF0
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H32T7V101':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x10000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H32T7V100':
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0X30000
        config['file_len'] = 0x30000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif (project_name == 'H15' or project_name == 'H16' or project_name == 'H28V101_CALIB' or project_name == 'H30'):
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0x8000
        config['file_len'] = 0x8000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H28V101_ADAPT':
        config['clip_addr_base'] = 0x8000
        config['bin_max_len'] = 0x7E50
        config['file_len'] = 0x7E50
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H28V200':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0xF000
        config['file_len'] = 0x8000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'PonLink10T28':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x4000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    elif project_name == 'H60LRV100':
        config['clip_addr_base'] = 0x0000
        config['bin_max_len'] = 0x10000
        config['file_len'] = 0x10000
        config['is_switch_bank'] = False
        config['version_len'] = 60
    else:
        return None
    return config

def HilinkHex2bin(PROJECT_MACRO,crc_type, path='./',is_work=False):
     """
     功能描述：hex2bin 主函数,将指定文件夹下面的hex文件按照既定的配置转换为同名的bin文件
     参数：外部调用时附带传入配置信息：项目名称
     返回值：处理结果描述字符串
     异常描述：传入参数错误无法生成,请在调用是正确传入项目名称
     修改记录
     """
     prj_config = get_config(PROJECT_MACRO)
     if prj_config is None:
         print('%s: error:can not find this \"%s\" project config! plz check input args' % (time.strftime("%H:%M:%S"), PROJECT_MACRO))
         return "NO config!"
     CLIP_ADDR_BASE = prj_config['clip_addr_base']
     BIN_MAX_LEN = prj_config['bin_max_len']
     FILE_LENS = prj_config['file_len']
     is_switch_bank = prj_config['is_switch_bank']
     VERSION_LEN = prj_config['version_len']
     #扫描当前文件夹里面的hex文件
     hex_file_list = scan_files(path,'.hex')
     #map_file_list = scan_files(path,'.MAP')
     if len(hex_file_list) == 0:
         print('%s: Warning:can\'t find any hex file in %s folder' % (time.strftime("%H:%M:%S"),(os.path.join(path))))
         return 'no_hex_file(s)'
     for _this_hex in hex_file_list:
        _f


def get_version(bin_array,lens=100):
    """从bin文件中获取版本信息"""
    str_rev = "rev"
    str_REV = "REV"
    str_sram = "sram"
    str_SRAM = "SRAM"
    str_V = "_v"
    _bin_with_version = bin_array[0:lens]
    strs_version_list = [chr(c) for c in _bin_with_version]
    strs_version = ''.join(strs_version_list)
    if str_rev in strs_version or str_REV in strs_version:
        #匹配到rev
        mod = re.compile("[0-9]\\.[0-9\\.]+")
        ver = mod.findall(strs_version)
        if len(ver) == 0:
            print('%s: Warning:二进制文件中版本信息匹配出错!!' % time.strftime("%H:%M:%S"))
            return ''
        return ver[0]
    elif str_sram in strs_version or str_SRAM in strs_version:
        #匹配到SRAM
        mod = re.compile("SRAM[0-9a-zA-Z\\.\\_]+")
        mod1 = re.compile("[0-9]{1,2}[0-9a-zA-Z\\.]+")
        ver = mod.findall(strs_version)
        if len(ver) == 0:
            print('%s: Warning:二进制文件中版本信息匹配出错!!' % time.strftime("%H:%M:%S"))
            return ''
        ver1 = mod1.findall(ver[0])
        if len(ver1) == 0:
            print('%s: Warning:二进制文件中版本信息匹配出错!!' % time.strftime("%H:%M:%S"))
            return ''
        return ver1[0]
    elif str_V in strs_version:
        mod = re.compile("_v[0-9a-zA-Z\\.]+")
        ver = mod.findall(strs_version)
        if len(ver) == 0:
            print('%s: Warning:二进制文件中版本信息匹配出错!!' % time.strftime("%H:%M:%S"))
            return ''
        else:
            return ver[0][2:]

    else:
        print('%s: Warning:未能发现二进制文件中包含版本信息!!' % time.strftime("%H:%M:%S"))
        return ''

    """
    Data字段表示数据的具体内容，描述方法仍是两个16进制的字符表示1字节的数据。此字段的长度由该记录的RecordLength决定，
    数据的解释取决于记录类型(RecordType)。Checksum字段为校验和。这个校验和是这么来的，将RecordMark(“:”)后的所有的
    数据按字节相加，即成对相加起来，然后模除256得到余数，再对这个余数求补码，最终得出的结果就是校验和。所以检测方法也
    很简单：在每一条记录内，将RecordMark(“:”)后的所有数据(包括Checksum)按字节相加后得到的8位数据为0，则说明数据无
    误，否则说明出错了。

    至于什么是拓展段地址记录、开始段地址记录、扩展线性地址记录、开始线性地址记录这里不做详细的介绍，在芯艺的《AVR单片机
    GCC程序设计》的附录部分有详细的说明。而在Arduino的HEX文件中，记录类型只有两种，数据记录和文件结束记录。所以Record
    Type这个字段的值不是0x00就是0x01。
    """

def hex_check_sum(hex_fmt_map):
    """hex 文件每行校验检测"""
    return

def hex_formator(hexLine):
    """
    功能描述：将传入的hex的一行按照hex 的格式分解元素并生成对应的字典
    参数：hex文件的原始行
    返回值：解析后的格式字典
    异常描述：
    修改记录
    """
    """
    Record structure

    A record (line of text) consists of six fields (parts) that appear in order from left to right:

    1. Start code, one character, an ASCII colon :.
    2. Byte count, two hex digits, indicating the number of bytes (hex digit pairs) in the data field.
       The maximum byte count is 255 (0xFF). 16 (0x10) and 32 (0x20) are commonly used byte counts.
    3. Address, four hex digits, representing the 16-bit beginning memory address offset of the data.
       The physical address of the data is computed by adding this offset to a previously established
       base address,thus allowing memory addressing beyond the 64 kilobyte limit of 16-bit addresses.
       The base address, which defaults to zero, can be changed by various types of records. Base
       addresses and address offsets are always expressed as big endian values.
    4. Record type (see record types below), two hex digits, 00 to 05, defining the meaning of the data field.
    5. Data, a sequence of n bytes of data, represented by 2n hex digits. Some records omit this field (n equals zero).
       The meaning and interpretation of data bytes depends on the application.
    6. Checksum, two hex digits, a computed value that can be used to verify the record has no errors.
    """
    _one_hex_map = {}
    if not hexLine.startswith(':'):
        return None
    else:
        _one_hex_map["start_code"] = hexLine[0:1]
        _one_hex_map["byte_count"] = hexLine[1:3]
        _one_hex_map["data_addrs"] = hexLine[3:7]
        _one_hex_map["recordtype"] = hexLine[7:9]
        _one_hex_map["main_datas"] = hexLine[9:(9 + (int(_one_hex_map["byte_count"],16) * 2))]
        _one_hex_map["check_sums"] = hexLine[-2:]
        return _one_hex_map

def crc_calc(_bin,_codelens,crc_type):
    """计算crc的值"""
    crc_table = get_crc_table(crc_type)
    c
ef save_bin(bin_array,filePath,fileName,ver,txt_nl=16):
    """
    功能描述：将bin格式文件按保存成bin文件在指定的路径和文件名,本功能会直接覆盖已有的文件
    参数1：bin_array bin文件数组
    参数2：filePath 保存的路径
    参数3：fileName 保存为文件名
    参数4：ver 版本信息
    返回值：保存文件成功：true 保存文件失败：false
    异常描述：保存失败
    修改记录:v1.2.0.0 增加保存带版本带日期的bin 和txt文件
    """
    f = []
    f.append(os.path.join(filePath,'%s%s'%(fileName,'.bin')))
    f.append(os.path.join(filePath,'%s%s%s%s%s'%(fileName,'_V',ver,time.strftime('_%Y%m%d_%H%M%S'),'.bin')))
    f_text = os.path.join(filePath,'%s%s%s%s%s'%(fileName,'_V',ver,time.strftime('_%Y%m%d_%H%M%S'),'.txt'))

    for fn in f:
        try:
            with open(fn, 'wb') as b:
                b.write(bytearray(bin_array))
                print('%s: save successfully--%s' % (time.strftime("%H:%M:%S"),os.path.basename(fn)))
        except:
            print('%s: Error:fail to save--%s!' % (time.strftime("%H:%M:%S"),os.path.basename(fn)))
            return False
    #将数据转为hex格式str
    hex_str_list = ['0x{:02X}'.format(b) for b in bin_array]
    hex_str_list_array = [hex_str_list[i:i + txt_nl] for i in range(0,len(hex_str_list),txt_nl)]
    try:
        with open(f_text,'w') as txt:
            for h_str_list in hex_str_list_array:
                for h in h_str_list:
                    txt.write(h + ', ')
                txt.write('\n')
        print('%s: save successfully--%s' % (time.strftime("%H:%M:%S"),os.path.basename(f_text)))
    except:
        print('%s: error:fail to save--%s!' % (time.strftime("%H:%M:%S"),os.path.basename(f_text)))
    return True

def scan_files(filePath,ext):
    """
    功能描述：扫描当前文件夹下面的指定的后缀文件的集合
    参数1：文件夹
    参数2：文件后缀名字
    返回值：文件路径list
    异常描述：对应后缀文件时返回None
    修改记录
    """
    _hex_file_list = []
    # print f_list
    for root, dirs, files in os.walk(filePath):
        for file in files:
            # os.path.splitext():分离文件名与扩展名
            if os.path.splitext(file)[1] == ext:
                _hex_file_list.append(os.path._getfullpathname(os.path.join(root, file)))
    if _hex_file_list.count == 0:
        return None
    else:
        return _hex_file_list
def args_parse():
    """
    运行参数提取
    """
    parse = argparse.ArgumentParser(description="将hex转换为二进制文件格式！请先输入Proj 信息(必要),再输入crc计算Type,crc不输入或者输入0代表不进行crc计算。")
    parse.add_argument("proj",choices=gl_proj_list,help="请务必输入对应的项目工程,工程要符合list,如需要,请在proj_config_list添加需要的工程信息,并新增Config！")
    parse.add_argument("--crc",type = int,default=0,choices=[8,16,32,0],help="请输入对应的crc类型,'0'代表不进行crc计算")
    args = parse.parse_args()
    proj,crctype = args.proj,args.crc
    return proj,crctype

if __name__ == "__main__":
    proj,crctype = args_parse()
    #proj,crctype = "H32T7V101",32
    HilinkHex2bin(proj,crctype)