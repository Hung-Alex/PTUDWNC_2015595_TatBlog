import { Card } from "react-bootstrap";
import React from "react";
import { useState,useEffect } from "react";
import { Link } from "react-router-dom";
import TagList from "./TagList";
import { isEmtyOrSpaces } from "../Utils/Utils";
import { getFeaturePosts } from "../Services/Widgets";
import PostItem from "./PostItem";


const FeaturePosts=()=>{

    const [postList,setPostList]=useState([]);
    useEffect(()=>{
        getFeaturePosts().then(data=>{
            if(data){
                setPostList(data);
            }else{
                setPostList([]);
            }
        })
    },[])

    return (
        <div className="mb-4">
          <h3 className="text-success mb-2">Top 3 bài viết được xem nhiều nhất</h3>
          {postList.length > 0 &&
            postList.map((item, index) => {
              return (
                <Card.Body key={index}>
                  <Card.Title>
                    <Link
                      to={`/blog/post?slug=${item.urlSlug}`}
                      title={item.title}
                      style={{ textDecoration: 'none' }}
                    >
                      {item.title}
                    </Link>
                  </Card.Title>
                </Card.Body>
              );
            })}
          <hr />
        </div>
      );
    };
    
    export default FeaturePosts;
    