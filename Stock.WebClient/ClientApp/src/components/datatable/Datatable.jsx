import "./datatable.scss";
import { DataGrid } from "@mui/x-data-grid";
import { Link } from "react-router-dom";
import { useState } from "react";
import Box from '@mui/material/Box';

const Datatable = (props) => {

  let width = 0;
  let height = 0;

  if(props.width && props.height){
    width = props.width;
    height = props.height;
  }else if(props.width){
    width = props.width;
    height = "100%";
  }else if(props.height){
    height = props.height;
    width = "100%";
  }else {
    width = "100%";
    height = "400px";
  }
  
  return (
    <Box sx={{width:width, height:height}}>
{/*       <div className="datatableTitle">
        Add New User
        <Link to="/users/new" className="link">
          Add New
        </Link>
      </div> */}
      <DataGrid
        
        rows={props.rowData}
        columns={props.columns}
      />
    </Box>
  );
};

export default Datatable;