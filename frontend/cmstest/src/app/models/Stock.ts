import Image from './Image';
import Accessory from './Accessory';

export default class Stock {
    public ID:number;
    public RegNumber:string;
    public Make:string;
    public Model:string;
    public ModelYear:string;
    public KMS:number;
    public Colour:string;
    public VIN:string;
    public RetailPrice:number;
    public CostPrice:number;
    public Accessories:Accessory[];
    public Images:Image[];
    public DTCreated:Date;
    public DTUpdated:Date;
}
