<?php
/**
 * Created by PhpStorm.
 * User: yoby
 * Date: 2018/8/24
 * Time: 23:31
 */
include "db.php";

if(1==$_POST['ajax']) {
    $pindex = max(1, intval($_POST['page']));  //页码
    $psize = $_POST['pagesize']; //每页显示数据
    $list = $db->fetchall("SELECT *  FROM " . $db->tablename('demo') . " WHERE 1=1  ORDER BY id asc LIMIT " . ($pindex - 1) * $psize . ',' . $psize);
    $total = $db->getcolumn('demo', array(), "count(*)");
    $arr = [
        'msg' => '请求成功',
        'code' => 200,
        "list" => $list,
        'total' => $total
    ];
    echo json_encode($arr);
}elseif(2==$_POST['ajax']){
    $pindex = max(1, intval($_POST['page']));
    $psize = $_POST['pagesize'];
    if($_POST['type']==2){
        $list[0] =[
            'id'=>99,
            'title'=>'测试',
            'createtime'=>time(),
            'phone'=>999,
            'fen'=>100

        ];
        $list[1] =[
            'id'=>98,
            'title'=>'测试1',
            'createtime'=>time(),
            'phone'=>999888,
            'fen'=>10

        ];
        $total=2;

    }else {
        $list = $db->fetchall("SELECT *  FROM " . $db->tablename('demo') . " WHERE 1=1  ORDER BY id asc LIMIT " . ($pindex - 1) * $psize . ',' . $psize);
        $total = $db->getcolumn('demo', array(), "count(*)");
    }
    $arr = [
        'msg' => '请求成功',
        'code' => 200,
        "list" => $list,
        'total' => $total
    ];
    echo json_encode($arr);
}elseif($_POST['ajax']==3){
    $areacode = (int)$_POST['code'];
    $name = $db->getcolumn("city",['code'=>$areacode],'name');
    exit(json_encode(["name"=>$name]));
}elseif($_POST['ajax']==4){
    $z = $_POST['py'];
    $list =$db->fetchall("select code,name,isok from ".$db->tablename('city')." where provincecode>0 and citycode>0 and areacode=0 and pinyin='$z' order by name");
    exit(json_encode(["list"=>$list]));
}elseif($_POST['ajax']==5){
    $kw = $_POST['kw'];
    $list = $db->fetchall("select code,name,isok from ".$db->tablename('city')." where provincecode>0 and citycode>0 and areacode=0 and name like '%$kw%' order by name");
    exit(json_encode(["list"=>$list]));
}elseif ($_POST['ajax']==6)
{//登录赞助表
    $pwd = $_POST['pwd'];
    if($pwd==''){
        exit(json_encode(["code"=>200,'msg'=>'<div class="weui-cell">
        <div class="weui-cell__hd"><label class="weui-label">金额</label></div>
        <div class="weui-cell__bd">
            <input class="weui-input" pattern="[0-9]*" placeholder="金额" type="number" id="money">
        </div>
    </div>
    <div class="weui-cell">
        <div class="weui-cell__hd"><label class="weui-label">赞助人</label></div>
        <div class="weui-cell__bd">
            <input class="weui-input"  placeholder="赞助人" type="text" id="zid">
        </div>
    </div>
    <div class="weui-cell">
        <div class="weui-cell__hd"><label class="weui-label">留言</label></div>
        <div class="weui-cell__bd">
            <input class="weui-input"  placeholder="留言" type="text" id="say">
        </div>
    </div>
    <div class="weui-btn-area">
    <a class="weui-btn weui-btn_primary" href="javascript:;" onclick="save()">保存</a>
    </div>']));
    }else{
        exit(json_encode(["code"=>400]));
    }
}elseif ($_POST['ajax']==7){
    $zid = $_POST['zid'];
    $money = $_POST['money'];
    $say = $_POST['say'];
    $createtime=time();
    if($zid==""){
      $arr=  ["code"=>400,'msg'=>'赞助人必填'];
        exit(json_encode($arr));
    }
    if($money==""){
        $arr=  ["code"=>400,'msg'=>'赞助金额必填'];
        exit(json_encode($arr));
    }
    $data = compact('zid','money','say','createtime');

    $db->insert('zanzhu',$data);
    $arr=  ["code"=>200,'msg'=>'提交成功'];
    exit(json_encode($arr));

}elseif($_POST['ajax']==8){
    $pindex = max(1, intval($_POST['page']));  //页码
    $psize = $_POST['pagesize']; //每页显示数据
    $list = $db->fetchall("SELECT *  FROM " . $db->tablename('zanzhu') . " WHERE 1=1  ORDER BY id desc LIMIT " . ($pindex - 1) * $psize . ',' . $psize);
    $total = $db->getcolumn('zanzhu', array(), "count(*)");
    $arr = [
        'msg' => '请求成功',
        'code' => 200,
        "list" => $list,
        'total' => $total
    ];
    echo json_encode($arr);
}