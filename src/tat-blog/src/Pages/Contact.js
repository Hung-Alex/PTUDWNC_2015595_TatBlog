import React,{useEffect} from "react";

const Contact=()=>{
    useEffect(()=>{
        document.title="Contact"
    },[]);
    return (
        <h1>
            Đây là trang liên lạc
        </h1>
    )
}
export default Contact