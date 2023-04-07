import React from "react";
import { isEmtyOrSpaces } from "../Utils/Utils";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import TagList from "./TagList";

const PostList = ({ postItem }) => {
  let imageUrl = isEmtyOrSpaces(postItem.imageUrl)
    ? process.env.PUBLIC_URL + "/images/image_1.png"
    : `${postItem.imageUrl}`;
  let postedDate = new Date(postItem.postedDate);

  return (
    <article  className="blog-entry mb-4">
      <Card>
        <div className="row g-0">
          <div className="col-md-4">
            <Card.Img variant="top" src={imageUrl} alt={postItem.title} />
          </div>

          <div className="col-md-8">
            <Card.Body>
              <Card.Title>{postItem.title}</Card.Title>
              <Card.Text>
                <small className="text-muted">Tác giả :</small>
                <span className="text-primary m-1">
                <Link to={`/blog/author/${String(postItem.authorName)}`}
                                    title={postItem.authorName}
                                    className="text-decoration-none">
                                        
                                        <span> {postItem.authorName} </span>
                                    </Link>
                 
                </span>
                <small className="text-muted">Chủ đề :</small>
                <span className="text-primary m-1">
                  {postItem.categoryName}
                  <Link to={`/blog/category/${String(postItem.categoryName)}`}
                                    title={postItem.categoryName}                                   
                                    className="text-decoration-none">
                                        
                                        <span> {postItem.categoryName} </span>
                                    </Link>
                </span>
              </Card.Text>
              <Card.Text>{postItem.shortDescription}</Card.Text>
              <div className="tag-list">
                <TagList tagList={postItem.tags} />
              </div>
              <div className="text-end">
                <Link
                  to={`/blog/post/${Number(postedDate.getFullYear())}/${Number(postedDate.getMonth())}/${Number(postedDate.getDay())}/${String(postItem.urlSlug)}`}
                  className="btn btn-primary"
                  title={postItem.Title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  );
};

export default PostList
