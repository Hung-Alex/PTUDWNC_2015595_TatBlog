import React from "react";
import { useState,useEffect } from "react";
import { getBestAuthor } from "../Services/Widgets";

import { isEmtyOrSpaces } from "../Utils/Utils";
import { Card ,Row} from "react-bootstrap";
import { Link } from "react-router-dom";
import TagList from "./TagList";

const BestAuthor = () => {
    const [authorsList, setAuthorsList] = useState([]);
  
    useEffect(() => {
      getBestAuthor().then((data) => {
        if (data) setAuthorsList(data);
        else setAuthorsList([]);
      });
    }, []);
  
    return (
      <div className="mb-4">
        <h3 className="text-success mb-2">Top 4 tác giả có nhiều bài viết nhất</h3>
        {authorsList.length > 0 &&
          authorsList.map((item, index) => {
            let imageUrl = isEmtyOrSpaces(item.imageUrl)
              ? import.meta.env.VITE_PUBLIC_URL + '/images/image_1.jpg'
              : `${item.imageUrl}`;
  
            return (
              <Card key={index} className="mt-3 p-2">
                <Row className="g-0">
                  <Card.Title>
                    <Link
                      to={`/blog/author?slug=${item.urlSlug}`}
                      style={{ textDecoration: 'none' }}
                      title={item.fullName}
                    >
                      {item.fullName}
                    </Link>
                  </Card.Title>
                  <Card.Img variant="top" src={imageUrl} alt="default" className="rounded w-50" />
                  <p>
                    Ngày bắt đầu làm việc: <i>{item.joinedDate}</i>
                  </p>
                  <h5>Email liên hệ:</h5>
                  {item.email}
                </Row>
              </Card>
            );
          })}
        <hr />
      </div>
    );
  };
  
  export default BestAuthor;