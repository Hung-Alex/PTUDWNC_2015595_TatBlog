import axios from "axios";

export async function getCategories(){
    try {
        const response=await axios.get('https://localhost:7001/api/categories/?PageSize=10&PageNumber=1');
        const data=response.data;
        if(data.isSuccess){
            return data.result;
        }else{
            return null;
        }
    } catch (error) {
        console.log('Error',error.message);
        return null;
    }
}

export async function getFeaturePosts(){
    try {
        const response=await axios.get('https://localhost:7001/api/posts/featured/3');
        const data=response.data;
        if(data.isSuccess){
            return data.result;
        }else{
            return null;
        }
    } catch (error) {
        console.log('Error',error.message);
        return null;
    }
}
export async function getRandomPost(limit = 5) {
    try {
      const response = await axios.get(
        `https://localhost:7001/api/posts/random/${limit}`
      );
      const data = response.data;
      if (data.isSuccess) return data.result;
      else return null;
    } catch (error) {
      console.log('Error', error.message);
      return null;
    }
  }

  export async function getArchivesPost(limit = 12) {
    try {
      const response = await axios.get(
        `https://localhost:7001/api/posts/archives/${limit}`
      );
      const data = response.data;
      if (data.isSuccess) return data.result;
      else return null;
    } catch (error) {
      console.log('Error', error.message);
      return null;
    }
  }
  
  export async function getTagCloud(limit = 12) {
    try {
      const response = await axios.get(
        `https://localhost:7001/api/tags/all`
      );
      const data = response.data;
      if (data.isSuccess) return data.result;
      else return null;
    } catch (error) {
      console.log('Error', error.message);
      return null;
    }
  }
  
  export async function getBestAuthor(limit = 12) {
    try {
      const response = await axios.get(
        `https://localhost:7001/api/authors/best/${limit}`
      );
      const data = response.data;
      if (data.isSuccess) return data.result;
      else return null;
    } catch (error) {
      console.log('Error', error.message);
      return null;
    }
  }