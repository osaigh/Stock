import "./datatable.scss";
import { DataGrid } from "@mui/x-data-grid";
import Box from '@mui/material/Box';

const Datatable = (props) => {

  const width = props.width || "100%";
  const height = props.height || (props.width ? "100%" : "400px");

  return (
    <Box sx={{width:width, height:height}}>
      <DataGrid
        rows={props.rowData}
        columns={props.columns}
      />
    </Box>
  );
};

export default Datatable;
