import { useEffect, useState } from 'react';
import Card from 'react-bootstrap/Card';
import { Link } from 'react-router-dom';
import { getTagCloud } from '../Services/Widgets';
import TagList from './TagList';
import React from 'react';
import TagItem from './TagItemt';
const TagCloud = () => {
  const [tagsList, setTagsList] = useState([]);

  useEffect(() => {
    getTagCloud().then((data) => {
      if (data) setTagsList(data);
      else setTagsList([]);
    });
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Các thẻ tag cloud</h3>
      {tagsList.length > 0 && (
        <div className="tag-list">
          <TagItem tagList={tagsList} />
        </div>
      )}
    </div>
  );
};

export default TagCloud;