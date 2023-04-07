import React from "react";
import SearchForm from "./SearchForm";
import CategorieWidget from "./CategoriesWidget";
import FeaturePosts from "./FeaturePosts";
import RandomPosts from "./RandomPosts";
import BestAuthor from "./BestAuthor";
import Archives from "./Archives";
import TagCloud from "./TagCloud";

const Sidebar=()=>{
    return (
        <div className="pt-4 ps-2">
           <SearchForm/>
            <CategorieWidget/>
            <h1>
                Bài viết nổi bật
                <FeaturePosts/>
                <RandomPosts/>
                <TagCloud/>
                <BestAuthor/>
                <Archives/>
            </h1>
            <h1>
                Đăng kí nhận tin mới
            </h1>
            <h1>
                Tag Cloud
            </h1>
        </div>
    )
}

export default Sidebar;